using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonArcSegment : IPolygonArcSegment
    {

        public Guid Id { get; }
        public IVertex StartVertex { get; set; }
        public IVertex EndVertex { get; set; }
        public IVertex Center { get; }
        public double Radius { get; }
        public double StartAngle { get; }
        public double SweepAngle { get; }
        public PolygonArcSegment(Guid id, IVertex startVertex, IVertex center, double radius, double startAngleDeg, double sweepAngleDeg)
        {
            Id = id;
            StartVertex = startVertex;
            Center = center;
            Radius = radius;
            StartAngle = startAngleDeg;
            SweepAngle = sweepAngleDeg;
            EndVertex = new Vertex(Guid.NewGuid());
            UpdateEndFromParameters();
        }

        public void UpdateEndFromParameters()
        {
            double endAngle = StartAngle + SweepAngle;
            double endVertexX = Center.Point.X + Radius * Math.Cos(endAngle);
            double endVertexY = Center.Point.Y + Radius * Math.Sin(endAngle);
            EndVertex.Point.X = endVertexX;
            EndVertex.Point.Y = endVertexY;
                
        }
    }
}
