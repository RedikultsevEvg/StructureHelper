using System.Collections.Generic;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IHasForceCombinations
    {
        /// <summary>
        /// Collection of force actions
        /// </summary>
        List<IForceAction> ForceActions { get; }
    }
}
