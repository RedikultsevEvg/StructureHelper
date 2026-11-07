using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    internal class CircleShapeCloneStrategy : ICloneStrategy<ICircleShape>
    {
        IUpdateStrategy<ICircleShape> updateStrategy;
        public ICircleShape GetClone(ICircleShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            updateStrategy ??= new CircleShapeUpdateStrategy();
            CircleShape clone = new(id);
            updateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
