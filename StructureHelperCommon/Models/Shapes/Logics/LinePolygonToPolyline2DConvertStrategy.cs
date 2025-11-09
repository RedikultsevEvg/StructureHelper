using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonToPolyline2DConvertStrategy : IObjectConvertStrategy<Polyline2D, ILinePolygonShape>
    {
        public double Dx { get; set; } = 0;
        public double Dy { get; set; } = 0;
        public double Scale { get; set; } = 1;

        public Polyline2D Convert(ILinePolygonShape linePolygon)
        {
            List<Polyline2DVertex> polylineVertices = [];
            foreach (var item in linePolygon.Vertices)
            {
                Polyline2DVertex vertex = new Polyline2DVertex((item.Point.X + Dx) * Scale, (item.Point.Y + Dy) * Scale);
                polylineVertices.Add(vertex);
            }
            Polyline2D polyline2D = new Polyline2D(polylineVertices)
            {
                IsClosed = linePolygon.IsClosed,
            };
            return polyline2D;
        }
    }
}
