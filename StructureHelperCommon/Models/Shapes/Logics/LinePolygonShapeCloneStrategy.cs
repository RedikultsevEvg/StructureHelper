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

        public ILinePolygonShape GetClone(ILinePolygonShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            LinePolygonShape polygon = new(id);
            UpdateStrategy.Update(polygon, sourceObject);
            return polygon;
        }
    }
}
