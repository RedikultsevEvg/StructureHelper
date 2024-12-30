using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Result of calculation of crack for specific result
    /// </summary>
    public interface IRebarCrackResult : IResult
    {
        /// <summary>
        /// Specific rebar primitive
        /// </summary>
        IRebarNdmPrimitive? RebarPrimitive { get; set; }
        /// <summary>
        /// Result of calculation of crack for long term
        /// </summary>
        CrackWidthRebarTupleResult? LongTermResult { get; set; }
        /// <summary>
        /// Result of calculation of crack for short term
        /// </summary>
        CrackWidthRebarTupleResult? ShortTermResult { get; set; }
    }
}