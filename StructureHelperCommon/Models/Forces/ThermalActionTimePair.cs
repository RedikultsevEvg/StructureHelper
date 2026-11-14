using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ThermalActionTimePair : IThermalActionTimePair
    {
        public Guid Id { get; }
        public double Time { get; set; }
        public IThermalActionTimeIndependend ThermalAction { get; set; }
        public ThermalActionTimePair(Guid Id, double time, IThermalActionTimeIndependend thermalAction)
        {
            CheckObject.ThrowIfNull(thermalAction, nameof(thermalAction));
            this.Id = Id;
            Time = time;
            ThermalAction = thermalAction;
        }

        public ThermalActionTimePair(double time, IThermalActionTimeIndependend thermalAction)
            : this(Guid.NewGuid(), time, thermalAction)
        {
        }

        public IThermalActionTimePair GetClone(IThermalActionTimePair sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
