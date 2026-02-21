using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalTShapeCloneStrategy : ICloneStrategy<IVerticalTShape>
    {
        private IUpdateStrategy<IVerticalTShape> updateStrategy;
        private IUpdateStrategy<IVerticalTShape> UpdateStrategy => updateStrategy ??= new VerticalTShapeUpdateStrategy();
        public IVerticalTShape GetClone(IVerticalTShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            VerticalTShape clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
