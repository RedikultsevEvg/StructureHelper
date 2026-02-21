using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes.ConvertLogics
{
    public class IShapeToPolygonConvertStrategy
        : IObjectConvertStrategy<ILinePolygonShape, IVerticalDoubleTShape>
    {
        public ILinePolygonShape Convert(IVerticalDoubleTShape source)
        {
            if (source == null)
                throw new StructureHelperException("I-shape is null.");

            if (source.FullHeight <= 0)
                throw new StructureHelperException("TotalHeight must be positive.");

            if (source.TopFlangeWidth <= 0 ||
                source.BottomFlangeWidth <= 0 ||
                source.WebThickness <= 0)
                throw new StructureHelperException("Widths must be positive.");

            if (source.TopFlangeThickness <= 0 ||
                source.BottomFlangeThickness <= 0)
                throw new StructureHelperException("Flange thickness must be positive.");

            var polygon = new LinePolygonShape();

            double halfHeight = source.FullHeight / 2.0;

            double halfTopWidth = source.TopFlangeWidth / 2.0;
            double halfBottomWidth = source.BottomFlangeWidth / 2.0;
            double halfWebThickness = source.WebThickness / 2.0;

            double topY = halfHeight;
            double bottomY = -halfHeight;

            double topFlangeBottomY = topY - source.TopFlangeThickness;
            double bottomFlangeTopY = bottomY + source.BottomFlangeThickness;

            // Start from bottom-left corner and go counter-clockwise

            // Bottom flange
            polygon.AddVertex(new Vertex(-halfBottomWidth, bottomY));
            polygon.AddVertex(new Vertex(halfBottomWidth, bottomY));
            polygon.AddVertex(new Vertex(halfBottomWidth, bottomFlangeTopY));
            polygon.AddVertex(new Vertex(halfWebThickness, bottomFlangeTopY));

            // Web (right side going up)
            polygon.AddVertex(new Vertex(halfWebThickness, topFlangeBottomY));
            polygon.AddVertex(new Vertex(halfTopWidth, topFlangeBottomY));

            // Top flange
            polygon.AddVertex(new Vertex(halfTopWidth, topY));
            polygon.AddVertex(new Vertex(-halfTopWidth, topY));
            polygon.AddVertex(new Vertex(-halfTopWidth, topFlangeBottomY));
            polygon.AddVertex(new Vertex(-halfWebThickness, topFlangeBottomY));

            // Web (left side going down)
            polygon.AddVertex(new Vertex(-halfWebThickness, bottomFlangeTopY));
            polygon.AddVertex(new Vertex(-halfBottomWidth, bottomFlangeTopY));

            polygon.IsClosed = true;

            return polygon;
        }
    }
}
