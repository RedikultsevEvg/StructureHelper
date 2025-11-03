using StructureHelperCommon.Models.States;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IExtendedForceTupleResult
    {
        IStateCalcTermPair StateCalcTermPair { get; set; }
        IForcesTupleResult ForcesTupleResut { get; set; }
    }
}
