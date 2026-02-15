using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonTranslateStrategy : IObjectConvertStrategy<ILinePolygonShape, ILinePolygonShape>
    {
        public double DeltaX { get; set; } = 0.0;
        public double DeltaY { get; set; } = 0.0;

        public ILinePolygonShape Convert(ILinePolygonShape source)
        {
            if (source is null)
            {
                throw new StructureHelperException("Source polygon is null.");
            }

            var result = new LinePolygonShape(Guid.NewGuid())
            {
                IsClosed = source.IsClosed
            };

            foreach (var v in source.Vertices)
            {
                double x = v.Point.X + DeltaX;
                double y = v.Point.Y + DeltaY;

                result.AddVertex(new Vertex(x, y));
            }
            return result;
        }
    }
}
