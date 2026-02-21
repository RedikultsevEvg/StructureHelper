using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalDoubleTShapeCloneStrategy : ICloneStrategy<IVerticalDoubleTShape>
    {
        private IUpdateStrategy<IVerticalDoubleTShape> updateStrategy;
        private IUpdateStrategy<IVerticalDoubleTShape> UpdateStrategy => updateStrategy ??= new VerticalDoubleTShapeUpdateStrategy();
        public IVerticalDoubleTShape GetClone(IVerticalDoubleTShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            VerticalDoubleTShape clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
