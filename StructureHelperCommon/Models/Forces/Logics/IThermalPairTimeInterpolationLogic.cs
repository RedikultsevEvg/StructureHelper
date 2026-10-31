using StructureHelperCommon.Models.Shapes;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Calculates the temperature of a time-dependent thermal action by interpolating between
    /// the temperature states defined for the specified time moments.
    /// </summary>
    public interface IThermalPairTimeInterpolationLogic
    {
        /// <summary>
        /// Returns the interpolated temperature at <paramref name="point"/> and <paramref name="time"/>, in kelvins.
        /// </summary>
        /// <exception cref="StructureHelperCommon.Infrastructures.Exceptions.StructureHelperException">
        /// Thrown when the collection is empty or <paramref name="time"/> is out of the specified time range.
        /// </exception>
        double GetTemperature(IReadOnlyList<IThermalActionTimePair> pairs, IPoint2D point, double time);
    }
}
