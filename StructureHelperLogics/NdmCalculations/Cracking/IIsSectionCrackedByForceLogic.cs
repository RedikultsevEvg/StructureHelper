using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for checking collection of ndms for appearance of crack
    /// </summary>
    public interface IIsSectionCrackedByForceLogic : IIsSectionCracked
    {
        /// <summary>
        /// Force tuple for checking of cracks appearence
        /// </summary>
        IForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Collection of ndms which is checking for cracking
        /// </summary>
        IEnumerable<INdm> CheckedNdmCollection { get; set; }
        /// <summary>
        /// Full ndms collection of cross-section
        /// </summary>
        IEnumerable<INdm> SectionNdmCollection { get; set; }
    }
}
