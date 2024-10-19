using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Input data for rebar crack calculator
    /// </summary>
    public interface IRebarCrackCalculatorInputData : IInputData
    {
        /// <summary>
        /// Long term rebar data
        /// </summary>
        IRebarCrackInputData? LongRebarData { get; set; }
        /// <summary>
        /// Short term rebar data
        /// </summary>
        IRebarCrackInputData? ShortRebarData { get; set; }
        /// <summary>
        /// Rebar primitive
        /// </summary>
        IRebarNdmPrimitive RebarPrimitive { get; set; }
        /// <summary>
        /// User settings for crack calculations
        /// </summary>
        IUserCrackInputData UserCrackInputData { get; set; }
    }
}