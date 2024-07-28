using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarCrackInputDataFactory
    {
        RebarPrimitive Rebar { get; set; }
        TupleCrackInputData InputData { get; set; }
        double LongLength { get; set; }
        double ShortLength { get; set; }

        RebarCrackCalculatorInputData GetInputData();
    }
}