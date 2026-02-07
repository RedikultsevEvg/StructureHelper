using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    internal class LinePolygonScalingStrategy : IObjectConvertStrategy<ILinePolygonShape, ILinePolygonShape>
    {
        public double ScaleX { get; set; } = 1.0;
        public double ScaleY { get; set; } = 1.0;

        public double CenterX { get; set; } = 0.0;
        public double CenterY { get; set; } = 0.0;

        public ILinePolygonShape Convert(ILinePolygonShape source)
        {
            if (source == null)
                throw new StructureHelperException("Source polygon is null.");

            if (ScaleX == 0 || ScaleY == 0)
                throw new StructureHelperException("Scale factor must not be zero.");

            var result = new LinePolygonShape(Guid.NewGuid())
            {
                IsClosed = source.IsClosed
            };

            foreach (var v in source.Vertices)
            {
                double x = CenterX + (v.Point.X - CenterX) * ScaleX;
                double y = CenterY + (v.Point.Y - CenterY) * ScaleY;

                result.AddVertex(new Vertex(x, y));
            }

            return result;
        }
    }
}
