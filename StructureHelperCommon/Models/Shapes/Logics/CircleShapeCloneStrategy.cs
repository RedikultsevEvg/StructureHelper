using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    internal class CircleShapeCloneStrategy : ICloneStrategy<ICircleShape>
    {
        IUpdateStrategy<ICircleShape> updateStrategy;

        public ICircleShape GetClone(ICircleShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            updateStrategy ??= new CircleShapeUpdateStrategy();
            CircleShape clone = new(id);
            updateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
