using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Result of crack calculation for specific force tuple
    /// </summary>
    public interface ITupleCrackResult : IResult 
    {
        /// <summary>
        /// Reference to input data for calculation
        /// </summary>
        TupleCrackInputData? InputData { get; set; }
        /// <summary>
        /// True if cross-section has been cracked under specific force combination
        /// </summary>
        bool IsCracked { get; set; }
        /// <summary>
        /// Result of calculation of crack width for long term force combination
        /// </summary>
        CrackWidthRebarTupleResult? LongTermResult { get; set; }
        /// <summary>
        /// Collection of resuls for specific rebars
        /// </summary>
        List<IRebarCrackResult>? RebarResults { get; }
        /// <summary>
        /// Result of calculation of crack width for short term force combination
        /// </summary>
        CrackWidthRebarTupleResult? ShortTermResult { get; set; }
    }
}