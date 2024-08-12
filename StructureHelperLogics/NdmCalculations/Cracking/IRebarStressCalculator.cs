using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarStressCalculator : ICalculator
    {
        IRebarStressCalculatorInputData InputData { get; set; }
    }
}