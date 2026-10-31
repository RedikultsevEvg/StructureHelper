using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Updates a (time, thermal action) pair. The thermal action is updated by the corresponding
    /// update strategy when <see cref="UpdateChildren"/> is enabled.
    /// </summary>
    public class ThermalActionTimePairUpdateStrategy : IParentUpdateStrategy<IThermalActionTimePair>
    {
        private readonly IUpdateStrategy<IThermalActionTimeIndependend> thermalActionUpdateStrategy;

        public ThermalActionTimePairUpdateStrategy()
            : this(new ThermalActionTimeIndependentUpdateStrategy())
        {
        }

        public ThermalActionTimePairUpdateStrategy(
            IUpdateStrategy<IThermalActionTimeIndependend> thermalActionUpdateStrategy)
        {
            this.thermalActionUpdateStrategy = thermalActionUpdateStrategy;
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(IThermalActionTimePair targetObject, IThermalActionTimePair sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            targetObject.Time = sourceObject.Time;

            if (UpdateChildren)
            {
                CheckObject.ThrowIfNull(sourceObject.ThermalAction, $"{nameof(sourceObject)}.{nameof(sourceObject.ThermalAction)}");
                CheckObject.ThrowIfNull(targetObject.ThermalAction, $"{nameof(targetObject)}.{nameof(targetObject.ThermalAction)}");
                thermalActionUpdateStrategy.Update(targetObject.ThermalAction, sourceObject.ThermalAction);
            }
        }
    }
}
