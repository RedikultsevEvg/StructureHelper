using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes.Logics;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeToDxfEntityConvertStrategy : IShapeConvertStrategy<EntityObject, IShape>
    {
        private const string ShapeTypeIsUnknown = ": Shape is unknown";
        public double Dx { get; set; } = 0.0;
        public double Dy { get; set; } = 0.0;
        public double Scale { get; set; } = 1.0;

        public EntityObject Convert(IShape source)
        {
            if (source is IRectangleShape rectangle)
            {
                var retangleConvertStrategy = new RectangleToLinePolygonConvertStrategy();
                var polyline2D = Convert(retangleConvertStrategy.Convert(rectangle));
                return polyline2D;
            }
            else if (source is ICircleShape circle)
            {
                var circleConvertStrategy = new CircleToDxfCircleConvertStrategy() { Dx = Dx, Dy = Dy, Scale = Scale };
                var circleEntity = circleConvertStrategy.Convert(circle);
                return circleEntity;
            }
            else if (source is ILinePolygonShape linePolygon)
            {
                var polygonConvertStrategy = new LinePolygonToPolyline2DConvertStrategy() { Dx = Dx, Dy = Dy, Scale = Scale };
                Polyline2D polyline2D = polygonConvertStrategy.Convert(linePolygon);
                return polyline2D;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ShapeTypeIsUnknown);
            }
        }
    }
}
