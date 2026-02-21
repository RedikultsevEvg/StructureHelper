using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class TrapezoidShapeCloneStrategy : ICloneStrategy<ITrapezoidShape>
    {
        private IUpdateStrategy<ITrapezoidShape> updateStrategy;
        private IUpdateStrategy<ITrapezoidShape> UpdateStrategy => updateStrategy ??= new TrapezoidShapeUpdateStrategy();
        public ITrapezoidShape GetClone(ITrapezoidShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            TrapezoidShape clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
