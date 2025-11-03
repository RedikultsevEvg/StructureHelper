using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorLogic : IValueDiagramCalculatorLogic
    {
        private ITriangulatePrimitiveLogic triangulateLogic;

        private IValueDiagramCalculatorResult result;
        public IValueDiagramCalculatorInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramCalculatorResult GetResult()
        {
            PrepareResult();
            GetPoints();
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
            var ndms = triangulateLogic.GetNdms();
            foreach (var forceAction in InputData.ForceActions)
            {
                var combination = forceAction.GetCombinations();
                List<IForceTuple> forceTuples = [];
                foreach (var action in combination)
                {
                    var actionCombination = action
                        .DesignForces
                        .Where(x => x.LimitState == InputData.StateTermPair.LimitState && x.CalcTerm == InputData.StateTermPair.CalcTerm);
                }
                ForceTupleInputData forceTupleInputData = new()
                {
                    NdmCollection = ndms,
                    Accuracy = new Accuracy(),
                    CheckStrainLimit = true,

                };
            }
        }

        private void GetPoints()
        {
            throw new NotImplementedException();
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
