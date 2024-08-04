using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICrackWidthTupleResult
    {
        /// <summary>
        /// Calculated crack width, m
        /// </summary>
        double CrackWidth { get; set; }
        /// <summary>
        /// True if actual crack width less than limit one
        /// </summary>
        bool IsCrackLessThanUltimate { get; }
        /// <summary>
        /// Limit crack width, m
        /// </summary>
        double UltimateCrackWidth { get; set; }
    }
}