using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeCloneStrategy : ICloneStrategy<IShape>
    {
        private ICloneStrategy<ICircleShape> circleCloneStrategy;
        private ICloneStrategy<IEllipseShape> ellipseCloneStrategy;
        private ICloneStrategy<ILinePolygonShape> linePolygonCloneStrategy;
        private ICloneStrategy<IRingShape> oShapeCloneStrategy;
        private ICloneStrategy<IRectangleShape> rectangleCloneStrategy;
        private ICloneStrategy<IVerticalTShape> tShapeCloneStrategy;

        private ICloneStrategy<ICircleShape> CircleCloneStrategy => circleCloneStrategy ??= new CircleShapeCloneStrategy();
        private ICloneStrategy<IEllipseShape> EllipseCloneStrategy => ellipseCloneStrategy ??= new EllipseShapeCloneStrategy();
        private ICloneStrategy<ILinePolygonShape> LinePolygonCloneStrategy => linePolygonCloneStrategy ??= new LinePolygonShapeCloneStrategy();
        private ICloneStrategy<IRingShape> OShapeCloneStrategy => oShapeCloneStrategy ??= new RingShapeCloneStrategy();
        private ICloneStrategy<IRectangleShape> RectangleCloneStrategy => rectangleCloneStrategy ??= new RectangleShapeCloneStrategy();
        private ICloneStrategy<IVerticalTShape> TShapeCloneStrategy => tShapeCloneStrategy ??= new VerticalTShapeCloneStrategy();


        public IShape GetClone(IShape sourceObject)
        {
            if (sourceObject is IRectangleShape rectangleShape)
            {
                return RectangleCloneStrategy.GetClone(rectangleShape);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                return EllipseCloneStrategy.GetClone(ellipseShape);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                return CircleCloneStrategy.GetClone(circleShape);
            }
            else if (sourceObject is IRingShape oshape)
            {
                return OShapeCloneStrategy.GetClone(oshape);
            }
            else if (sourceObject is ILinePolygonShape linePolygon)
            {
                return LinePolygonCloneStrategy.GetClone(linePolygon);
            }
            else if (sourceObject is IVerticalTShape verticalTShape)
            {
                return TShapeCloneStrategy.GetClone(verticalTShape);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
