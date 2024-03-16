using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Buckling;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : IForceCalculator, IHasActionByResult
    {
        static readonly ForceCalculatorUpdateStrategy updateStrategy = new();
        private readonly IForceTupleCalculator forceTupleCalculator;
        private ForcesResults result;
        private IProcessorLogic<IForceTuple> eccentricityLogic;
        private ForceTupleBucklingLogic bucklingLogic;

        public string Name { get; set; }
        public List<LimitStates> LimitStatesList { get; private set; }
        public List<CalcTerms> CalcTermsList { get; private set; }
        public List<IForceAction> ForceActions { get; private set; }
        public List<INdmPrimitive> Primitives { get; private set; }
        public IResult Result { get; private set; }
        public ICompressedMember CompressedMember { get; private set; }
        public IAccuracy Accuracy { get; set; }
        public List<IForceCombinationList> ForceCombinationLists { get; private set; }
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                Result = new ForcesResults()
                {
                    IsValid = false,
                    Description = checkResult
                };
                return;
            }
            else
            {
                GetCombinations();
                CalculateResult();
            }
        }

        private void CalculateResult()
        {
            result = new ForcesResults()
            {
                IsValid = true
            };
            foreach (var combination in ForceCombinationLists)
            {
                foreach (var tuple in combination.DesignForces)
                {
                    var limitState = tuple.LimitState;
                    var calcTerm = tuple.CalcTerm;
                    if (LimitStatesList.Contains(limitState) & CalcTermsList.Contains(calcTerm))
                    {

                        IForcesTupleResult tupleResult;
                        try
                        {
                            tupleResult = ProcessNdmResult(combination, tuple);
                        }
                        catch(Exception ex)
                        {
                            tupleResult = new ForcesTupleResult()
                            {
                                IsValid = false,
                                Description = string.Empty + ex,
                                DesignForceTuple = tuple
                            };
                        }
                        result.ForcesResultList.Add(tupleResult);
                        ActionToOutputResults?.Invoke(result);
                    }
                }
            }
            Result = result;
        }

        private IForcesTupleResult ProcessNdmResult(IForceCombinationList combination, IDesignForceTuple tuple)
        {
            IForcesTupleResult tupleResult;
            LimitStates limitState = tuple.LimitState;
            CalcTerms calcTerm = tuple.CalcTerm;
            var ndms = NdmPrimitivesService.GetNdms(Primitives, limitState, calcTerm);
            IPoint2D point2D;
            IProcessorLogic<IForceTuple> forcelogic = new ForceTupleCopier(tuple.ForceTuple);
            if (combination.SetInGravityCenter == true)
            {
                var (Cx, Cy) = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
                point2D = new Point2D() { X = Cx, Y = Cy };
                forcelogic = new ForceTupleMoveToPointDecorator(forcelogic) { Point2D = point2D};
            }
            var newTuple = forcelogic.GetValue();
            TraceLogger?.AddMessage("Input force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            if (CompressedMember.Buckling == true)
            {
                if (newTuple.Nz >= 0d)
                {
                    TraceLogger?.AddMessage(string.Format("Second order effect is not considered as Nz={0} >= 0", newTuple.Nz));
                    tupleResult = GetForceResult(limitState, calcTerm, ndms, newTuple);
                }
                else
                {
                    tupleResult = ProcessCompressedMember(combination, tuple, ndms, newTuple);
                }             
            }
            else
            {
                if (newTuple.Nz < 0d)
                {
                    string message = string.Format("Second order effect is not considered, despite force Nz={0}", newTuple.Nz);
                    TraceLogger?.AddMessage(message, TraceLogStatuses.Warning);
                }
                tupleResult = GetForceResult(limitState, calcTerm, ndms, newTuple);
            }
            return tupleResult;
        }

        private IForcesTupleResult ProcessCompressedMember(IForceCombinationList combination, IDesignForceTuple tuple, List<INdm> ndms, IForceTuple newTuple)
        {
            IForcesTupleResult tupleResult;
            LimitStates limitState = tuple.LimitState;
            CalcTerms calcTerm = tuple.CalcTerm;

            TraceLogger?.AddMessage("Get eccentricity for full load");
            eccentricityLogic = new ProcessEccentricity(CompressedMember, ndms, newTuple)
            {
                TraceLogger = TraceLogger ?? null
            };
            newTuple = eccentricityLogic.GetValue();
            var buclingInputData = new BucklingInputData()
            {
                Combination = combination,
                LimitState = limitState,
                CalcTerm = calcTerm,
                Ndms = ndms,
                ForceTuple = newTuple
            };
            bucklingLogic = new ForceTupleBucklingLogic(buclingInputData)
            {
                CompressedMember = CompressedMember,
                Accuracy = Accuracy,
                Primitives = Primitives,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            var buckResult = bucklingLogic.GetForceTupleByBuckling();
            if (buckResult.IsValid == true)
            {
                newTuple = buckResult.Value;
            }
            else
            {
                return new ForcesTupleResult()
                {
                    IsValid = false,
                    DesignForceTuple = tuple,
                    Description = buckResult.Description,
                };
            }
            TraceLogger?.AddMessage(string.Intern("Result of second order was obtained succesfully, new force combination was obtained"));
            tupleResult = GetForceResult(limitState, calcTerm, ndms, newTuple);
            return tupleResult;
        }

        private IForcesTupleResult GetForceResult(LimitStates limitState, CalcTerms calcTerm, List<INdm> ndms, IForceTuple newTuple)
        {
            TraceLogger?.AddMessage("Calculation of cross-section is started");
            var tupleResult = GetPrimitiveStrainMatrix(ndms, newTuple, Accuracy);
            tupleResult.DesignForceTuple.LimitState = limitState;
            tupleResult.DesignForceTuple.CalcTerm = calcTerm;
            tupleResult.DesignForceTuple.ForceTuple = newTuple;
            return tupleResult;
        }

        private string CheckInputData()
        {
            string result = "";
            try
            {
                NdmPrimitivesService.CheckPrimitives(Primitives);
            }
            catch (Exception ex)
            {
                result += ex;
            }
            if (ForceActions.Count == 0)
            {
                result += "Calculator does not contain any forces \n";
            }
            if (LimitStatesList.Count == 0)
            {
                result += "Calculator does not contain any limit states \n";
            }
            if (CalcTermsList.Count == 0)
            {
                result += "Calculator does not contain any duration \n";
            }
            return result;
        }

        public ForceCalculator(IForceTupleCalculator forceTupleCalculator)
        {
            this.forceTupleCalculator = forceTupleCalculator;
            SetDefaultProperties();
        }

        public ForceCalculator() : this(new ForceTupleCalculator())
        {           
        }

        private void SetDefaultProperties()
        {
            ForceActions = new List<IForceAction>();
            Primitives = new List<INdmPrimitive>();
            CompressedMember = new CompressedMember()
            {
                Buckling = false
            };
            Accuracy = new Accuracy()
            {
                IterationAccuracy = 0.001d,
                MaxIterationCount = 1000
            };
            LimitStatesList = new List<LimitStates>()
            {
                LimitStates.ULS,
                LimitStates.SLS
            };
            CalcTermsList = new List<CalcTerms>()
            {
                CalcTerms.ShortTerm,
                CalcTerms.LongTerm
            };
        }
        private void GetCombinations()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            foreach (var item in ForceActions)
            {
                ForceCombinationLists.Add(item.GetCombinations());
            }
        }

        private IForcesTupleResult GetPrimitiveStrainMatrix(IEnumerable<INdm> ndmCollection, IForceTuple tuple, IAccuracy accuracy)
        {
            var inputData = new ForceTupleInputData()
            {
                NdmCollection = ndmCollection,
                Tuple = tuple,
                Accuracy = accuracy
            };
            var calculator = forceTupleCalculator.Clone() as IForceTupleCalculator;
            calculator.InputData = inputData;
            if (TraceLogger is not null)
            {
                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger();
            }
            calculator.Run();
            return calculator.Result as IForcesTupleResult;
        }

        public object Clone()
        {
            var newCalculator = new ForceCalculator();
            updateStrategy.Update(newCalculator, this);
            return newCalculator;
        }
    }
}
