using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Buckling;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculatorLogic : IForceCalculatorLogic
    {
        private ForcesResults result;
        private IProcessorLogic<IForceTuple> eccentricityLogic;
        private ForceTupleBucklingLogic bucklingLogic;
        private ITriangulatePrimitiveLogic triangulateLogic;
        public IForceCalculatorInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }


        public ForcesResults GetForcesResults()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            GetCombinations();
            CalculateResult();
            return result;
        }

        private void CalculateResult()
        {
            result = new ForcesResults()
            {
                IsValid = true
            };
            foreach (var combination in InputData.ForceCombinationLists)
            {
                foreach (var tuple in combination.DesignForces)
                {
                    var limitState = tuple.LimitState;
                    var calcTerm = tuple.CalcTerm;
                    if (InputData.LimitStatesList.Contains(limitState) & InputData.CalcTermsList.Contains(calcTerm))
                    {

                        IForcesTupleResult tupleResult;
                        try
                        {
                            tupleResult = ProcessNdmResult(combination, tuple);
                        }
                        catch (Exception ex)
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
        }

        private IForcesTupleResult ProcessNdmResult(IForceCombinationList combination, IDesignForceTuple tuple)
        {
            IForcesTupleResult tupleResult;
            LimitStates limitState = tuple.LimitState;
            CalcTerms calcTerm = tuple.CalcTerm;
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = limitState,
                CalcTerm = calcTerm,
                TraceLogger = TraceLogger
            };
            var ndms = triangulateLogic.GetNdms();
            IPoint2D point2D;
            IProcessorLogic<IForceTuple> forcelogic = new ForceTupleCopier(tuple.ForceTuple);
            if (combination.SetInGravityCenter == true)
            {
                var (Cx, Cy) = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
                point2D = new Point2D() { X = Cx, Y = Cy };
                forcelogic = new ForceTupleMoveToPointDecorator(forcelogic) { Point2D = point2D };
            }
            var newTuple = forcelogic.GetValue();
            TraceLogger?.AddMessage("Input force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            if (InputData.CompressedMember.Buckling == true)
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
            eccentricityLogic = new ProcessEccentricity(InputData.CompressedMember, ndms, newTuple)
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
                CompressedMember = InputData.CompressedMember,
                Accuracy = InputData.Accuracy,
                Primitives = InputData.Primitives,
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

            string message = string.Intern("Result of second order was obtained succesfully, new force combination was obtained");
            TraceLogger?.AddMessage(message);
            tupleResult = GetForceResult(limitState, calcTerm, ndms, newTuple);
            return tupleResult;
        }

        private IForcesTupleResult GetForceResult(LimitStates limitState, CalcTerms calcTerm, List<INdm> ndms, IForceTuple newTuple)
        {
            TraceLogger?.AddMessage("Calculation of cross-section is started");
            var tupleResult = GetPrimitiveStrainMatrix(ndms, newTuple, InputData.Accuracy);
            tupleResult.DesignForceTuple.LimitState = limitState;
            tupleResult.DesignForceTuple.CalcTerm = calcTerm;
            tupleResult.DesignForceTuple.ForceTuple = newTuple;
            return tupleResult;
        }


        private void GetCombinations()
        {
            InputData.ForceCombinationLists = new List<IForceCombinationList>();
            foreach (var item in InputData.ForceActions)
            {
                InputData.ForceCombinationLists.Add(item.GetCombinations());
            }
        }

        private IForcesTupleResult GetPrimitiveStrainMatrix(IEnumerable<INdm> ndmCollection, IForceTuple tuple, IAccuracy accuracy)
        {
            var inputData = new ForceTupleInputData()
            {
                NdmCollection = ndmCollection,
                ForceTuple = tuple,
                Accuracy = accuracy
            };
            var calculator = new ForceTupleCalculator();
            calculator.InputData = inputData;
            if (TraceLogger is not null)
            {
                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger();
            }
            calculator.Run();
            return calculator.Result as IForcesTupleResult;
        }


    }
}
