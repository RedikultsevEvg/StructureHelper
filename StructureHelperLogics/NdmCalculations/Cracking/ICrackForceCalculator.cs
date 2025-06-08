using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackForceCalculator : ILogicCalculator
    {
        ICrackForceCalculatorInputData InputData { get; set; }
    }
}