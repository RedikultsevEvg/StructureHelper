using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace StructureHelperCommon.Models.Shapes
{
    public static class PolygonGeometryUtils
    {
        public static ILinePolygonShape GetTratsfromedPolygon(ILinePolygonShape polygon, double dx, double dy)
        {
            ILinePolygonShape newPolygon = new LinePolygonShape(Guid.Empty);
            var updateLogic = new LinePolygonShapeUpdateStrategy();
            updateLogic.Update(newPolygon, polygon);
            foreach (var item in newPolygon.Vertices)
            {
                item.Point.X += dx;
                item.Point.Y += dy;
            }
            return newPolygon;
        }
        public static bool DoPolygonsEdgesIntersect(ILinePolygonShape polygon)
        {
            var vertices = polygon.Vertices;
            int n = vertices.Count;

            if (n < 4)
                return false; // Triangles cannot self-intersect

            for (int i = 0; i < n; i++)
            {
                var a1 = vertices[i].Point;
                var a2 = vertices[(i + 1) % n].Point;

                for (int j = i + 1; j < n; j++)
                {
                    var b1 = vertices[j].Point;
                    var b2 = vertices[(j + 1) % n].Point;

                    // Skip adjacent edges (they share a vertex)
                    if (AreAdjacent(i, j, n, polygon.IsClosed))
                        continue;

                    if (SegmentsIntersect(a1, a2, b1, b2))
                        return true;
                }
            }

            return false;
        }

        private static bool AreAdjacent(int i, int j, int n, bool isClosed)
        {
            // Edges (i,i+1) and (j,j+1) are adjacent if:
            if (j == i) return true;
            if (j == i + 1) return true;
            if (i == j + 1) return true;

            // Special case: first and last edges in closed polygon
            if (isClosed &&
                ((i == 0 && j == n - 1) || (j == 0 && i == n - 1)))
                return true;

            return false;
        }

        private static bool SegmentsIntersect(IPoint2D p1, IPoint2D p2, IPoint2D q1, IPoint2D q2)
        {
            return (Orientation(p1, p2, q1) * Orientation(p1, p2, q2) < 0) &&
                   (Orientation(q1, q2, p1) * Orientation(q1, q2, p2) < 0);
        }

        private static int Orientation(IPoint2D a, IPoint2D b, IPoint2D c)
        {
            double val = (b.Y - a.Y) * (c.X - b.X) -
                         (b.X - a.X) * (c.Y - b.Y);

            if (Math.Abs(val) < 1e-12) return 0;  // Collinear
            return (val > 0) ? 1 : -1;            // Clockwise / Counter-clockwise
        }
    }

}
