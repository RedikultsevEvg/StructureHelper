using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Offsets a closed polygon by a given distance. Positive distances move the contour
    /// outward, negative distances move it inward.
    /// </summary>
    public class LinePolygonOffsetStrategy
        : IObjectConvertStrategy<ILinePolygonShape, ILinePolygonShape>
    {
        private const double Epsilon = 1e-12;

        public double Distance { get; }

        public LinePolygonOffsetStrategy(double distance)
        {
            Distance = distance;
        }

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

            if (Math.Abs(Distance) < Epsilon)
            {
                foreach (IVertex vertex in source.Vertices)
                {
                    result.AddVertex(new Vertex(vertex.Point.X, vertex.Point.Y));
                }
                return result;
            }

            if (!source.IsClosed)
            {
                throw new StructureHelperException("Offset is supported only for closed polygons.");
            }

            IReadOnlyList<IVertex> vertices = source.Vertices;
            if (vertices.Count < 3)
            {
                throw new StructureHelperException("A polygon must contain at least three vertices.");
            }

            double signedArea = GetSignedArea(vertices);
            if (Math.Abs(signedArea) < Epsilon)
            {
                throw new StructureHelperException("Cannot determine the offset direction of a degenerate polygon.");
            }

            // For a counter-clockwise polygon the outward normal of an edge is (dy, -dx);
            // for a clockwise polygon the direction is reversed.
            double orientation = signedArea > 0d ? 1d : -1d;
            int count = vertices.Count;

            for (int i = 0; i < count; i++)
            {
                IVertex previous = vertices[(i - 1 + count) % count];
                IVertex current = vertices[i];
                IVertex next = vertices[(i + 1) % count];

                (double x, double y) previousNormal = GetOutwardNormal(previous.Point, current.Point, orientation);
                (double x, double y) nextNormal = GetOutwardNormal(current.Point, next.Point, orientation);

                double denominator = 1d + previousNormal.x * nextNormal.x + previousNormal.y * nextNormal.y;
                double offsetX;
                double offsetY;
                if (Math.Abs(denominator) < Epsilon)
                {
                    // A 180-degree turn: fall back to the normal of the incoming edge.
                    offsetX = previousNormal.x * Distance;
                    offsetY = previousNormal.y * Distance;
                }
                else
                {
                    double scale = Distance / denominator;
                    offsetX = (previousNormal.x + nextNormal.x) * scale;
                    offsetY = (previousNormal.y + nextNormal.y) * scale;
                }

                result.AddVertex(new Vertex(
                    current.Point.X + offsetX,
                    current.Point.Y + offsetY));
            }

            return result;
        }

        private static (double x, double y) GetOutwardNormal(
            IPoint2D start,
            IPoint2D end,
            double orientation)
        {
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length < Epsilon)
            {
                return (0d, 0d);
            }
            return (orientation * dy / length, orientation * -dx / length);
        }

        private static double GetSignedArea(IReadOnlyList<IVertex> vertices)
        {
            double area = 0d;
            for (int i = 0; i < vertices.Count; i++)
            {
                IPoint2D current = vertices[i].Point;
                IPoint2D next = vertices[(i + 1) % vertices.Count].Point;
                area += current.X * next.Y - next.X * current.Y;
            }
            return area * 0.5d;
        }
    }
}
