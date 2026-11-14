using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class EllipseShapeCloneStrategy : ICloneStrategy<IEllipseShape>
    {
        IUpdateStrategy<IEllipseShape> updateStrategy;
        IUpdateStrategy<IEllipseShape> UpdateStrategy => updateStrategy ??= new EllipseShapeUpdateStrategy();

        public IEllipseShape GetClone(IEllipseShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            EllipseShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
