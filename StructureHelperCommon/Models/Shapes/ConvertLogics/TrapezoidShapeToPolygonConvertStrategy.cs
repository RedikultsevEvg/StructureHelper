using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public class TrapezoidShapeToPolygonConvertStrategy
        : IObjectConvertStrategy<ILinePolygonShape, ITrapezoidShape>
    {
        public ILinePolygonShape Convert(ITrapezoidShape source)
        {
            if (source == null)
                throw new StructureHelperException("Trapezoid shape is null.");

            if (source.BottomBase <= 0)
                throw new StructureHelperException("Bottom base must be positive.");

            if (source.TopBase <= 0)
                throw new StructureHelperException("Top base must be positive.");

            if (source.Height <= 0)
                throw new StructureHelperException("Height must be positive.");

            var polygon = new LinePolygonShape();

            double halfBottom = source.BottomBase / 2.0;
            double halfTop = source.TopBase / 2.0;

            double topCenterX = source.TopBaseOffset;

            // Bottom base (centered at X=0, Y=0)
            var bottomLeft = new Vertex(-halfBottom, 0);
            var bottomRight = new Vertex(halfBottom, 0);

            // Top base (shifted by offset, Y=Height)
            var topLeft = new Vertex(topCenterX - halfTop, source.Height);
            var topRight = new Vertex(topCenterX + halfTop, source.Height);

            // Counter-clockwise order
            polygon.AddVertex(bottomLeft);
            polygon.AddVertex(bottomRight);
            polygon.AddVertex(topRight);
            polygon.AddVertex(topLeft);

            polygon.IsClosed = true;

            return polygon;
        }
    }
}
