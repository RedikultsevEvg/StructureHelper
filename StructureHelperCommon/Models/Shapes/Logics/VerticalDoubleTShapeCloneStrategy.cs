using StructureHelperCommon.Infrastructures.Enums;
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
        public IVerticalDoubleTShape GetClone(IVerticalDoubleTShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            VerticalDoubleTShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
