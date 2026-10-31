using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Thermal action whose temperature state changes with time. The state is defined by a set of
    /// (time, temperature field) pairs and is interpolated between them.
    /// </summary>
    public interface IThermalActionTimeList : IThermalAction
    {
        /// <summary>
        /// Temperature states ordered by time. The order of the collection is not required to be sorted.
        /// </summary>
        List<IThermalActionTimePair> TimePairs { get; }
    }
}
