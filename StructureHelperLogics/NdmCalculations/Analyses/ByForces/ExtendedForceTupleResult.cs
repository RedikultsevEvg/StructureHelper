using StructureHelperCommon.Models.States;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ExtendedForceTupleResult : IExtendedForceTupleResult
    {
        public IStateCalcTermPair StateCalcTermPair { get; set; } = new StateCalcTermPair();
        public IForcesTupleResult ForcesTupleResut { get; set; }
    }
}
