using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IIsSectionCracked : ILogic
    {
        /// <summary>
        /// Returns result of checking of cracks appearence
        /// </summary>
        /// <returns>True if Checked collectition contains cracked elements</returns>
        bool IsSectionCracked();
    }
}