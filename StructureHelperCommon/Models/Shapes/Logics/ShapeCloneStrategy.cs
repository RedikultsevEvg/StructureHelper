using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeCloneStrategy : ICloneStrategy<IShape>
    {
        private IShapeCloneStrategyContainer strategyContainer;
        private IShapeCloneStrategyContainer StrategyContainer => strategyContainer ??= new ShapeCloneStrategyContainer();



        public IShape GetClone(IShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }

            if (sourceObject is IRectangleShape rectangleShape)
            {
                return StrategyContainer.RectangleCloneStrategy.GetClone(rectangleShape, cloneMode, cloneDepth);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                return StrategyContainer.EllipseCloneStrategy.GetClone(ellipseShape, cloneMode, cloneDepth);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                return StrategyContainer.CircleCloneStrategy.GetClone(circleShape, cloneMode, cloneDepth);
            }
            else if (sourceObject is IRingShape oshape)
            {
                return StrategyContainer.OShapeCloneStrategy.GetClone(oshape, cloneMode, cloneDepth);
            }
            else if (sourceObject is ILinePolygonShape linePolygon)
            {
                return StrategyContainer.LinePolygonCloneStrategy.GetClone(linePolygon, cloneMode, cloneDepth);
            }
            else if (sourceObject is ITrapezoidShape trapezoidShape)
            {
                return StrategyContainer.TrapezoidShapeCloneStrategy.GetClone(trapezoidShape, cloneMode, cloneDepth);
            }
            else if (sourceObject is IVerticalTShape verticalTShape)
            {
                return StrategyContainer.TShapeCloneStrategy.GetClone(verticalTShape, cloneMode, cloneDepth);
            }
            else if (sourceObject is IVerticalDoubleTShape verticalDoubleTShape)
            {
                return StrategyContainer.DoubleTShapeCloneStrategy.GetClone(verticalDoubleTShape, cloneMode, cloneDepth);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
