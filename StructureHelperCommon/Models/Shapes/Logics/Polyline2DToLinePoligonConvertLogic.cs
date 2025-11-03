using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class Polyline2DToLinePoligonConvertLogic : IObjectConvertStrategy<LinePolygonShape, Polyline2D>
    {
        private const double metresToMillimeters = 1000.0;
        private IArcFlatteningOption options = new ArcFlatteningOption();

        public LinePolygonShape Convert(Polyline2D source)
        {
            var polygon = new LinePolygonShape(Guid.NewGuid());

            for (int i = 0; i < source.Vertexes.Count; i++)
            {
                var v1 = source.Vertexes[i];
                var v2 = source.Vertexes[(i + 1) % source.Vertexes.Count];

                polygon.AddVertex(new Vertex(v1.Position.X / metresToMillimeters, v1.Position.Y / metresToMillimeters));

                double bulge = v1.Bulge;
                if (Math.Abs(bulge) < 1e-9)
                    continue;
                ProcessBulge(polygon, v1, v2, bulge);
            }

            polygon.IsClosed = source.IsClosed;
            return polygon;
        }

        private void ProcessBulge(LinePolygonShape polygon, Polyline2DVertex v1, Polyline2DVertex v2, double bulge)
        {
            var p1 = v1.Position;
            var p2 = v2.Position;

            double chord = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double theta = 4 * Math.Atan(Math.Abs(bulge));
            double radius = chord / (2 * Math.Sin(theta / 2));

            double dir = bulge > 0 ? 1 : -1;

            // Compute arc center
            double mx = (p1.X + p2.X) / 2;
            double my = (p1.Y + p2.Y) / 2;
            double nx = -(p2.Y - p1.Y) / chord;
            double ny = (p2.X - p1.X) / chord;
            double d = radius * Math.Cos(theta / 2);
            double cx = mx + dir * nx * d;
            double cy = my + dir * ny * d;

            double a1 = Math.Atan2(p1.Y - cy, p1.X - cx);
            double a2 = Math.Atan2(p2.Y - cy, p2.X - cx);

            double totalAngle = a2 - a1;
            if (dir > 0 && totalAngle < 0) totalAngle += 2 * Math.PI;
            if (dir < 0 && totalAngle > 0) totalAngle -= 2 * Math.PI;

            double stepAngle = GetEffectiveStepAngle(radius, Math.Abs(totalAngle), options);
            int segments = Math.Max(2, (int)Math.Ceiling(Math.Abs(totalAngle) / stepAngle));

            for (int j = 1; j < segments; j++)
            {
                double a = a1 + totalAngle * j / segments;
                double x = cx + radius * Math.Cos(a);
                double y = cy + radius * Math.Sin(a);
                polygon.AddVertex(new Vertex(x / metresToMillimeters, y / metresToMillimeters));
            }
        }

        private static double GetEffectiveStepAngle(double radius, double arcAngle, IArcFlatteningOption options)
        {
            // compute angle from each criterion
            double byAngle = options.AngleStepRadians;
            double byCount = arcAngle / options.SegmentCount;
            double byLength = 2 * Math.Asin(options.MaxSegmentLength * metresToMillimeters / (2 * radius));

            return options.Mode switch
            {
                ArcFlatteningMode.ByAngleStep => byAngle,
                ArcFlatteningMode.BySegmentCount => byCount,
                ArcFlatteningMode.ByMaxSegmentLength => byLength,
                ArcFlatteningMode.Combined => Math.Min(byAngle, Math.Min(byCount, byLength)),
                _ => byAngle
            };
        }
    }
}
