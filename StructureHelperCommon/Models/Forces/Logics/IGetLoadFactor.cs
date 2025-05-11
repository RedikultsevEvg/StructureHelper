using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implements logic for calculating factor of load
    /// </summary>
    public interface IGetLoadFactor
    {
        /// <summary>
        /// Returns factor for load
        /// </summary>
        /// <returns>Factor for load</returns>
        double GetFactor();
    }
}
