using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonRotateStrategy
        : IObjectConvertStrategy<ILinePolygonShape, ILinePolygonShape>
    {
        public double AngleRadians { get; }
        public double CenterX { get; }
        public double CenterY { get; }

        public LinePolygonRotateStrategy(
            double angleRadians,
            double centerX = 0.0,
            double centerY = 0.0)
        {
            AngleRadians = angleRadians;
            CenterX = centerX;
            CenterY = centerY;
        }

        public ILinePolygonShape Convert(ILinePolygonShape source)
        {
            if (source == null)
                throw new StructureHelperException("Source polygon is null.");

            var result = new LinePolygonShape(Guid.NewGuid())
            {
                IsClosed = source.IsClosed
            };

            double cos = Math.Cos(AngleRadians);
            double sin = Math.Sin(AngleRadians);

            foreach (var v in source.Vertices)
            {
                double x = v.Point.X;
                double y = v.Point.Y;

                double dx = x - CenterX;
                double dy = y - CenterY;

                double rotatedX = CenterX + dx * cos - dy * sin;
                double rotatedY = CenterY + dx * sin + dy * cos;

                var point = new Point2D(rotatedX, rotatedY);
                var vertex = new Vertex(Guid.NewGuid()) { Point = point };

                result.AddVertex(vertex);
            }

            return result;
        }
    }
}
