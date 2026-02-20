using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class CircleShapeToPolygonConvertStrategy : IObjectConvertStrategy<ILinePolygonShape, ICircleShape>
    {
        bool IsCounterClockWise { get; set; } = true;
        public int Segments { get; set; } = 16;

        public CircleShapeToPolygonConvertStrategy(bool isCounterClockWise, int segments = 16)
        {
            if (segments < 3)
                throw new StructureHelperException("Segments must be >= 3.");

            IsCounterClockWise = isCounterClockWise;
            Segments = segments;
        }

        public ILinePolygonShape Convert(ICircleShape circle)
        {
            if (circle == null)
            {
                throw new StructureHelperException("Circle is null.");
            }

            LinePolygonShape polygon = new LinePolygonShape();
            double radius = circle.Diameter / 2.0;

            for (int i = 0; i < Segments; i++)
            {
                double factor = 1.0;
                if (IsCounterClockWise == false) { factor = - 1.0; }
                double angle = 2.0 * Math.PI * i / Segments * factor;

                double x = radius * Math.Cos(angle);
                double y = radius * Math.Sin(angle);

                var point = new Point2D(x, y);

                var vertex = new Vertex(Guid.NewGuid()) { Point = point};

                polygon.AddVertex(vertex);
            }
            polygon.IsClosed = true;
            return polygon;
        }
    }
}
