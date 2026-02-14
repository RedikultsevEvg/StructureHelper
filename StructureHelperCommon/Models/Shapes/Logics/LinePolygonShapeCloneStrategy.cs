using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonShapeCloneStrategy : ICloneStrategy<ILinePolygonShape>
    {
        private IUpdateStrategy<ILinePolygonShape> updateStrategy;
        private IUpdateStrategy<ILinePolygonShape> UpdateStrategy => updateStrategy ??= new LinePolygonShapeUpdateStrategy();

        public ILinePolygonShape GetClone(ILinePolygonShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            LinePolygonShape polygon = new(Guid.NewGuid());
            UpdateStrategy.Update(polygon, sourceObject);
            return polygon;
        }
    }
}
