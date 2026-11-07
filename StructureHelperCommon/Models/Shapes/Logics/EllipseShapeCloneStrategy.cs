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
        public IEllipseShape GetClone(IEllipseShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            EllipseShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
