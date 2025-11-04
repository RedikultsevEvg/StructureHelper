using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Buckling;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <inheritdoc/>
    public class ForceCalculatorLogic : IForceCalculatorLogic
    {
        private ForceCalculatorResult result;
        private IProcessorLogic<IForceTuple> eccentricityLogic;
        private ForceTupleBucklingLogic bucklingLogic;
        private ITriangulatePrimitiveLogic triangulateLogic;
        private List<IForceCombinationList> combinationLists;

        public IForceCalculatorInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }


        public ForceCalculatorResult GetForcesResults()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceInputData();
            GetCombinations();
            CalculateResult();
            TraceResult();
            return result;
        }

        private void TraceResult()
        {
            TraceForcesResultLogic traceLogic = new()
            {
                Collection = result.ForcesResultList
            };
            List<ITraceLoggerEntry> traceEntries = traceLogic.GetTraceEntries();
            foreach (var item in traceEntries)
            {
                TraceLogger?.AddEntry(item);
            }
        }

        private void TraceInputData()
        {
            ITraceEntityLogic tracelogic = new TracePrimitiveFactory()
            {
                Collection = InputData.Primitives
            };
            tracelogic.AddEntriesToTraceLogger(TraceLogger);
            tracelogic = new TraceMaterialsFactory()
            {
                Collection = InputData.Primitives.Select(x => x.NdmElement.HeadMaterial).Distinct()
            };
            tracelogic.AddEntriesToTraceLogger(TraceLogger);
        }

        private void CalculateResult()
        {
            result = new ForceCalculatorResult()
            {
                IsValid = true
            };
            foreach (var combination in combinationLists)
            {
                foreach (var tuple in combination.DesignForces)
                {
                    var limitState = tuple.LimitState;
                    var calcTerm = tuple.CalcTerm;
                    if (InputData.LimitStatesList.Contains(limitState) & InputData.CalcTermsList.Contains(calcTerm))
                    {

                        ExtendedForceTupleCalculatorResult extendedResult = new();
                        extendedResult.StateCalcTermPair.LimitState = limitState;
                        extendedResult.StateCalcTermPair.CalcTerm = calcTerm;
                        try
                        {
                            IForceTupleCalculatorResult tupleResult = ProcessNdmResult(combination, tuple);
                            extendedResult.IsValid = tupleResult.IsValid;
                            extendedResult.ForcesTupleResult = tupleResult;
                        }
                        catch (Exception ex)
                        {
                            extendedResult.IsValid = false;
                            extendedResult.Description += ex.Message;
                        }
                        result.ForcesResultList.Add(extendedResult);
                        ActionToOutputResults?.Invoke(result);
                    }
                }
            }
        }

        private IForceTupleCalculatorResult ProcessNdmResult(IForceCombinationList combination, IDesignForceTuple tuple)
        {
            IForceTupleCalculatorResult tupleResult;
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
                    tupleResult = GetForceResult(ndms, newTuple);
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
                tupleResult = GetForceResult(ndms, newTuple);
            }
            return tupleResult;
        }

        private IForceTupleCalculatorResult ProcessCompressedMember(IForceCombinationList combination, IDesignForceTuple tuple, List<INdm> ndms, IForceTuple newTuple)
        {
            IForceTupleCalculatorResult tupleResult;
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
                return new ForceTupleCalculatorResult()
                {
                    IsValid = false,
                    ForceTuple = tuple.ForceTuple,
                    Description = buckResult.Description,
                };
            }

            string message = string.Intern("Result of second order was obtained successfully, new force combination was obtained");
            TraceLogger?.AddMessage(message);
            tupleResult = GetForceResult(ndms, newTuple);
            return tupleResult;
        }

        private IForceTupleCalculatorResult GetForceResult(List<INdm> ndms, IForceTuple newTuple)
        {
            TraceLogger?.AddMessage("Calculation of cross-section is started");
            var tupleResult = GetPrimitiveStrainMatrix(ndms, newTuple, InputData.Accuracy);
            return tupleResult;
        }


        private void GetCombinations()
        {
            combinationLists = new List<IForceCombinationList>();
            foreach (var item in InputData.ForceActions)
            {
                combinationLists.AddRange(item.GetCombinations());
            }
        }

        private IForceTupleCalculatorResult GetPrimitiveStrainMatrix(IEnumerable<INdm> ndmCollection, IForceTuple tuple, IAccuracy accuracy)
        {
            var inputData = new ForceTupleInputData()
            {
                NdmCollection = ndmCollection,
                ForceTuple = tuple,
                Accuracy = accuracy,
                CheckStrainLimit = InputData.CheckStrainLimit,
            };
            var calculator = new ForceTupleCalculator();
            calculator.InputData = inputData;
            if (TraceLogger is not null)
            {
                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger();
            }
            calculator.Run();
            return calculator.Result as IForceTupleCalculatorResult;
        }


    }
}
