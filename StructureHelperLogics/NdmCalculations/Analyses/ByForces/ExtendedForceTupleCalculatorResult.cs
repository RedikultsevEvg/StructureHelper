using StructureHelperCommon.Models.States;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ExtendedForceTupleCalculatorResult : IExtendedForceTupleCalculatorResult
    {
        public IStateCalcTermPair StateCalcTermPair { get; set; } = new StateCalcTermPair(Guid.NewGuid());
        public IForceTupleCalculatorResult ForcesTupleResult { get; set; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
