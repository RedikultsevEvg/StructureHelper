using NLog.Config;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public class VerticalTShapeToPolygonConvertStrategy : IObjectConvertStrategy<ILinePolygonShape, IVerticalTShape>
    {
        public ILinePolygonShape Convert(IVerticalTShape shape)
        {
            LinePolygonShape polygon = new();

            double h = shape.FullHeight;
            double hf = shape.FlangeHeight;
            double hw = h - hf;

            double bw = shape.WebWidth / 2.0;
            double bf = shape.FlangeWidth / 2.0;
            double dx = shape.FlangeXOffset;

            // Bottom-left of web
            polygon.AddVertex(V(-bw, 0));

            // Bottom-right of web
            polygon.AddVertex(V(bw, 0));

            // Right side of web up to flange
            polygon.AddVertex(V(bw, hw));

            // Right-bottom of flange
            polygon.AddVertex(V(dx + bf, hw));

            // Right-top of flange
            polygon.AddVertex(V(dx + bf, h));

            // Left-top of flange
            polygon.AddVertex(V(dx - bf, h));

            // Left-bottom of flange
            polygon.AddVertex(V(dx - bf, hw));

            // Left side of web down
            polygon.AddVertex(V(-bw, hw));

            polygon.IsClosed = true;
            return polygon;
        }

        private IVertex V(double x, double y) => new Vertex(x, y);
    }

}
