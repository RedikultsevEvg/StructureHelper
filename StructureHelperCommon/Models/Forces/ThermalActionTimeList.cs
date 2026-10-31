using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
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

        /// <inheritdoc/>
        public object Clone()
        {
            updateStrategy ??= new ThermalActionTimeListUpdateStrategy();
            var clone = new ThermalActionTimeList();
            updateStrategy.Update(clone, this);
            return clone;
        }
    }
}
