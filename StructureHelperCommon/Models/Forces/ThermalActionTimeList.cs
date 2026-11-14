using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ThermalActionTimeList : IThermalActionTimeList
    {
        private readonly IThermalPairTimeInterpolationLogic interpolationLogic;
        private IParentUpdateStrategy<IThermalActionTimeList> updateStrategy;

        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public List<IThermalActionTimePair> TimePairs { get; } = [];
        public ThermalActionTimeList()
            : this(Guid.NewGuid())
        {
        }

        public ThermalActionTimeList(Guid id)
            : this(id, new ThermalPairTimeInterpolationLogic())
        {
        }

        public ThermalActionTimeList(Guid id, IThermalPairTimeInterpolationLogic interpolationLogic)
        {
            Id = id;
            this.interpolationLogic = interpolationLogic ?? throw new ArgumentNullException(nameof(interpolationLogic));
        }

        public IThermalActionTimeList GetClone(IThermalActionTimeList sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var clone = new ThermalActionTimeList(id);
            updateStrategy ??= new ThermalActionTimeListUpdateStrategy() { UpdateChildren = false};
            updateStrategy.Update(clone, this);
            clone.TimePairs.Clear();
            foreach (var item in sourceObject.TimePairs)
            {
                clone.TimePairs.Add(item.GetClone(item, cloneMode, cloneDepth));
            }
            return clone;
        }
    }
}
