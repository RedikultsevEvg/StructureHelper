using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Calculates width of cracks for cross-section by collection of combination of forces
    /// </summary>
    public interface ICrackCalculator : ICalculator
    {
        /// <summary>
        /// Input data for crack calculations
        /// </summary>
        ICrackCalculatorInputData InputData { get; set; }
    }
}