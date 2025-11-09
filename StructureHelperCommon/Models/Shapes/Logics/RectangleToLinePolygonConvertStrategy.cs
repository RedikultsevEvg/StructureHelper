using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class RectangleToLinePolygonConvertStrategy : IShapeConvertStrategy<LinePolygonShape, IRectangleShape>
    {
        private IRectangleShape rectangle;

        public double Dx { get; set; } = 0.0;
        public double Dy { get; set; } = 0.0;
        public double Scale { get; set; } = 1.0;


        public LinePolygonShape Convert(IRectangleShape source)
        {
            rectangle = source;
            LinePolygonShape polygon = new(Guid.NewGuid());
            polygon.AddVertex(GetVertex(-1.0, 1.0));
            polygon.AddVertex(GetVertex(1.0, 1.0));
            polygon.AddVertex(GetVertex(1.0, -1.0));
            polygon.AddVertex(GetVertex(-1.0, -1.0));
            polygon.IsClosed = true;
            return polygon;
        }

        private Vertex GetVertex(double kx, double ky)
        {
            double x = (kx * rectangle.Width / 2 + Dx) * Scale;
            double y = (ky * rectangle.Height / 2 + Dy) * Scale;
            return new(x, y);
        }
    }
}
