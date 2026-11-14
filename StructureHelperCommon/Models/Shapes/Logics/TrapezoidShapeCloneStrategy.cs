using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class TrapezoidShapeCloneStrategy : ICloneStrategy<ITrapezoidShape>
    {
        private IUpdateStrategy<ITrapezoidShape> updateStrategy;
        private IUpdateStrategy<ITrapezoidShape> UpdateStrategy => updateStrategy ??= new TrapezoidShapeUpdateStrategy();

        public ITrapezoidShape GetClone(ITrapezoidShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            TrapezoidShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
