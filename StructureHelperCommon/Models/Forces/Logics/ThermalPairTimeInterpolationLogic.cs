using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ThermalPairTimeInterpolationLogic : IThermalPairTimeInterpolationLogic
    {
        private IThermalIndependentLogic thermalIndependentLogic;

        public ThermalPairTimeInterpolationLogic(IThermalIndependentLogic thermalIndependentLogic)
        {
            this.thermalIndependentLogic = thermalIndependentLogic;
        }

        public ThermalPairTimeInterpolationLogic() : this (new ThermalIndependentLogic())
        {
            
        }

        public double GetTemperature(IReadOnlyList<IThermalActionTimePair> pairs, IPoint2D point, double time)
        {
            CheckObject.ThrowIfNull(pairs, nameof(pairs));
            CheckObject.ThrowIfNull(point, nameof(point));

            if (pairs.Count == 0)
            {
                throw new StructureHelperException("Thermal action does not contain any time points.");
            }

            List<IThermalActionTimePair> orderedPairs = pairs.OrderBy(x => x.Time).ToList();
            double firstTime = orderedPairs[0].Time;
            double lastTime = orderedPairs[^1].Time;

            if (double.IsNaN(time) || time < firstTime || time > lastTime)
            {
                throw new StructureHelperException(
                    $"Time {time} is out of the range [{firstTime}; {lastTime}] specified for the thermal action.");
            }

            for (int i = 0; i < orderedPairs.Count; i++)
            {
                if (orderedPairs[i].Time == time)
                {
                    return GetTemperature(orderedPairs[i], point);
                }

                if (i > 0 && orderedPairs[i].Time > time)
                {
                    IThermalActionTimePair previous = orderedPairs[i - 1];
                    IThermalActionTimePair next = orderedPairs[i];
                    double previousTemperature = GetTemperature(previous, point);
                    double nextTemperature = GetTemperature(next, point);
                    double factor = (time - previous.Time) / (next.Time - previous.Time);
                    return previousTemperature + (nextTemperature - previousTemperature) * factor;
                }
            }

            return GetTemperature(orderedPairs[^1], point);
        }

        private double GetTemperature(IThermalActionTimePair pair, IPoint2D point)
        {
            return thermalIndependentLogic.GetTemperature(pair.ThermalAction, point);
        }
    }
}
