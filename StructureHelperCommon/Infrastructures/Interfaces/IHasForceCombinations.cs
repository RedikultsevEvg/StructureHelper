using System.Collections.Generic;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IHasForceCombinations
    {
        List<IForceAction> ForceActions { get; }
    }
}
