using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implements logic for calculating factor of load
    /// </summary>
    public interface IGetLoadFactor
    {
        double GetFactor();
    }
}
