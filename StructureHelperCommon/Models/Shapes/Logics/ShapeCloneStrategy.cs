using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeCloneStrategy : ICloneStrategy<IShape>
    {
        private IShapeCloneStrategyContainer strategyContainer;
        private IShapeCloneStrategyContainer StrategyContainer => strategyContainer ??= new ShapeCloneStrategyContainer();


        public IShape GetClone(IShape sourceObject)
        {
            if (sourceObject is IRectangleShape rectangleShape)
            {
                return StrategyContainer.RectangleCloneStrategy.GetClone(rectangleShape);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                return StrategyContainer.EllipseCloneStrategy.GetClone(ellipseShape);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                return StrategyContainer.CircleCloneStrategy.GetClone(circleShape);
            }
            else if (sourceObject is IRingShape oshape)
            {
                return StrategyContainer.OShapeCloneStrategy.GetClone(oshape);
            }
            else if (sourceObject is ILinePolygonShape linePolygon)
            {
                return StrategyContainer.LinePolygonCloneStrategy.GetClone(linePolygon);
            }
            else if (sourceObject is ITrapezoidShape trapezoidShape)
            {
                return StrategyContainer.TrapezoidShapeCloneStrategy.GetClone(trapezoidShape);
            }
            else if (sourceObject is IVerticalTShape verticalTShape)
            {
                return StrategyContainer.TShapeCloneStrategy.GetClone(verticalTShape);
            }
            else if (sourceObject is IVerticalDoubleTShape verticalDoubleTShape)
            {
                return StrategyContainer.DoubleTShapeCloneStrategy.GetClone(verticalDoubleTShape);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
