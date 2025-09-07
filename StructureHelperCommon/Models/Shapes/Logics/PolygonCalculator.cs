using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonCalculator : IPolygonCalculator
    {
        public double GetPerimeter(IPolygonShape polygon)
        {
            if (polygon.Vertices.Count < 2)
                return 0;

            double perimeter = 0;

            for (int i = 0; i < polygon.Vertices.Count - 1; i++)
            {
                perimeter += Distance(polygon.Vertices[i].Point, polygon.Vertices[i + 1].Point);
            }

            if (polygon.IsClosed && polygon.Vertices.Count > 2)
            {
                perimeter += Distance(polygon.Vertices[^1].Point, polygon.Vertices[0].Point);
            }

            return perimeter;
        }

        public double GetArea(IPolygonShape polygon)
        {
            if (!polygon.IsClosed || polygon.Vertices.Count < 3)
                return 0;

            // Shoelace formula
            double sum = 0;
            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                var p1 = polygon.Vertices[i].Point;
                var p2 = polygon.Vertices[(i + 1) % polygon.Vertices.Count].Point;
                sum += (p1.X * p2.Y - p2.X * p1.Y);
            }

            return Math.Abs(sum) / 2.0;
        }

        public bool ContainsPoint(IPolygonShape polygon, IPoint2D point)
        {
            if (!polygon.IsClosed || polygon.Vertices.Count < 3)
                return false;

            // Ray casting algorithm
            bool inside = false;
            int j = polygon.Vertices.Count - 1;

            for (int i = 0; i < polygon.Vertices.Count; i++)
            {
                var pi = polygon.Vertices[i].Point;
                var pj = polygon.Vertices[j].Point;

                if (((pi.Y > point.Y) != (pj.Y > point.Y)) &&
                    (point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X))
                {
                    inside = !inside;
                }
                j = i;
            }

            return inside;
        }

        private double Distance(IPoint2D p1, IPoint2D p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

}
