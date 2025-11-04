using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.States;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IExtendedForceTupleCalculatorResult : IResult
    {
        IStateCalcTermPair StateCalcTermPair { get; set; }
        IForceTupleCalculatorResult ForcesTupleResult { get; set; }
    }
}
