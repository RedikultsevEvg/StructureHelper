using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonShapeCloneStrategy : ICloneStrategy<ILinePolygonShape>
    {
        private IUpdateStrategy<ILinePolygonShape> updateStrategy;
        private IUpdateStrategy<ILinePolygonShape> UpdateStrategy => updateStrategy ??= new LinePolygonShapeUpdateStrategy();

        public ILinePolygonShape GetClone(ILinePolygonShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            LinePolygonShape polygon = new(id);
            UpdateStrategy.Update(polygon, sourceObject);
            return polygon;
        }
    }
}
