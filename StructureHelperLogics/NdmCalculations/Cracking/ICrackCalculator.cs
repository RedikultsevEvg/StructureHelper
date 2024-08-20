using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackCalculator : ICalculator
    {
        ICrackCalculatorInputData InputData { get; set; }
    }
}