using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public class RectangleShapeToPolygonConvertStrategy
        : IObjectConvertStrategy<ILinePolygonShape, IRectangleShape>
    {
        public ILinePolygonShape Convert(IRectangleShape source)
        {
            if (source == null)
                throw new StructureHelperException("Rectangle shape is null.");

            var polygon = new LinePolygonShape();

            double halfWidth = source.Width / 2.0;
            double halfHeight = source.Height / 2.0;

            polygon.AddVertex(new Vertex(-halfWidth, -halfHeight));
            polygon.AddVertex(new Vertex(halfWidth, -halfHeight));
            polygon.AddVertex(new Vertex(halfWidth, halfHeight));
            polygon.AddVertex(new Vertex(-halfWidth, halfHeight));

            polygon.IsClosed = true;

            return polygon;
        }
    }
}
