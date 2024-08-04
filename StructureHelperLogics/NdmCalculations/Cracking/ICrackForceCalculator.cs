using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackForceCalculator : ICalculator
    {
        ICrackForceCalculatorInputData InputData { get; set; }
        string Name { get; set; }
    }
}