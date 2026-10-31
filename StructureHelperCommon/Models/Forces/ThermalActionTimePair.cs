using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ThermalActionTimePair : IThermalActionTimePair
    {
        public ThermalActionTimePair(double time, IThermalActionTimeIndependend thermalAction)
        {
            CheckObject.ThrowIfNull(thermalAction, nameof(thermalAction));
            Time = time;
            ThermalAction = thermalAction;
        }

        /// <inheritdoc/>
        public double Time { get; set; }

        /// <inheritdoc/>
        public IThermalActionTimeIndependend ThermalAction { get; set; }
    }
}
