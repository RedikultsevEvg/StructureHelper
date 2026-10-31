using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Updates a time-dependent thermal action. Every pair is cloned together with its temperature
    /// state when <see cref="UpdateChildren"/> is enabled.
    /// </summary>
    public class ThermalActionTimeListUpdateStrategy : IParentUpdateStrategy<IThermalActionTimeList>
    {
        private readonly IUpdateStrategy<IThermalActionTimePair> pairUpdateStrategy;

        public ThermalActionTimeListUpdateStrategy()
            : this(new ThermalActionTimePairUpdateStrategy())
        {
        }

        public ThermalActionTimeListUpdateStrategy(IUpdateStrategy<IThermalActionTimePair> pairUpdateStrategy)
        {
            this.pairUpdateStrategy = pairUpdateStrategy;
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(IThermalActionTimeList targetObject, IThermalActionTimeList sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject))
            {
                return;
            }

            targetObject.Name = sourceObject.Name;

            if (UpdateChildren == false)
            {
                return;
            }

            targetObject.TimePairs.Clear();
            foreach (IThermalActionTimePair sourcePair in sourceObject.TimePairs)
            {
                IThermalActionTimeIndependend thermalAction = CloneThermalAction(sourcePair.ThermalAction);
                var targetPair = new ThermalActionTimePair(sourcePair.Time, thermalAction);
                pairUpdateStrategy.Update(targetPair, sourcePair);
                targetObject.TimePairs.Add(targetPair);
            }
        }

        private static IThermalActionTimeIndependend CloneThermalAction(
            IThermalActionTimeIndependend thermalAction)
        {
            CheckObject.ThrowIfNull(thermalAction, nameof(thermalAction));
            if (thermalAction is ICloneable cloneable)
            {
                return (IThermalActionTimeIndependend)cloneable.Clone();
            }
            throw new StructureHelperException(
                $"{nameof(IThermalActionTimeIndependend)} implementation does not support cloning.");
        }
    }
}
