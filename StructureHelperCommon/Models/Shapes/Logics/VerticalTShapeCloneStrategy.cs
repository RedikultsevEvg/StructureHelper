using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalTShapeCloneStrategy : ICloneStrategy<IVerticalTShape>
    {
        private IUpdateStrategy<IVerticalTShape> updateStrategy;
        private IUpdateStrategy<IVerticalTShape> UpdateStrategy => updateStrategy ??= new VerticalTShapeUpdateStrategy();

        public VerticalTShapeCloneStrategy(IUpdateStrategy<IVerticalTShape> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }
        public VerticalTShapeCloneStrategy()
        {
            
        }

        public IVerticalTShape GetClone(IVerticalTShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            VerticalTShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
