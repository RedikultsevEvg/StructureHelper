using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Sections.Logics;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmPrimitives;
using System;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorLogic : IValueDiagramCalculatorLogic
    {
        private readonly IValueDiagramEntityLogic entityLogic = new ValueDiagramEntityLogic();
        private ITriangulatePrimitiveLogic triangulateLogic;
        private List<INdm> ndms;
        private IValueDiagramCalculatorResult result;
        public IValueDiagramCalculatorInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramCalculatorResult GetResult()
        {
            PrepareResult();
            GetEntitiesResults();
            CalculateTupleResults();
            return result;
        }

        private void CalculateTupleResults()
        {
            triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = InputData.StateTermPair.LimitState,
                CalcTerm = InputData.StateTermPair.CalcTerm,
                TraceLogger = TraceLogger
            };
            ndms = triangulateLogic.GetNdms();
            foreach (var forceAction in InputData.ForceActions)
            {
                var combination = forceAction.GetCombinations();
                List<IForceTuple> forceTuples = [];
                foreach (var action in combination)
                {
                    var forceTuple = action
                        .DesignForces
                        .Single(x => x.LimitState == InputData.StateTermPair.LimitState && x.CalcTerm == InputData.StateTermPair.CalcTerm)
                        .ForceTuple;


                    IPoint2D point2D;
                    IProcessorLogic<IForceTuple> forcelogic = new ForceTupleCopier(forceTuple);
                    if (action.SetInGravityCenter == true)
                    {
                        var (Cx, Cy) = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
                        point2D = new Point2D() { X = Cx, Y = Cy };
                        forcelogic = new ForceTupleMoveToPointDecorator(forcelogic) { Point2D = point2D };
                    }
                    var newTuple = forcelogic.GetValue();
                    GetForceTupleResult(forceAction, newTuple);
                }

            }
        }

        private void GetForceTupleResult(IForceAction forceAction, IForceTuple forceTuple)
        {
            ForceTupleInputData forceTupleInputData = new()
            {
                ForceTuple = forceTuple,
                NdmCollection = ndms,
                Accuracy = new Accuracy() { IterationAccuracy = 0.001, MaxIterationCount = 1000},
                CheckStrainLimit = InputData.CheckStrainLimit,
            };
            ForceTupleCalculator calculator = new ForceTupleCalculator()
            {
                InputData = forceTupleInputData,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(100)
            };
            calculator.Run();
            var tupleResult = calculator.Result as IForceTupleCalculatorResult;
            if (tupleResult.IsValid == false)
            {
                TraceLogger?.AddMessage($"Result is not valid for action {forceAction.Name}: {tupleResult.Description}", TraceLogStatuses.Error);
            }
            result.ForceTupleResults.Add(tupleResult);
        }

        private void GetEntitiesResults()
        {
            var entities = InputData.Digrams
                .Where(x => x.IsTaken == true);
            foreach (var entity in entities)
            {
                entityLogic.ValueDiagramEntity = entity;
                entityLogic.Run();
                result.EntityResults.Add(entityLogic.Result);
            }
        }

        private void PrepareResult()
        {
            result = new ValueDiagramCalculatorResult()
            {
                InputData = InputData,
            };
        }
    }
}
