using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarStressCalculator : ILogicCalculator
    {
        IRebarStressCalculatorInputData InputData { get; set; }
    }
}