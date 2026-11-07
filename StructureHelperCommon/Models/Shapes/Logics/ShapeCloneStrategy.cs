using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class ShapeCloneStrategy : ICloneStrategy<IShape>
    {
        private IShapeCloneStrategyContainer strategyContainer;
        private IShapeCloneStrategyContainer StrategyContainer => strategyContainer ??= new ShapeCloneStrategyContainer();


        public IShape GetClone(IShape sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }

            if (sourceObject is IRectangleShape rectangleShape)
            {
                return StrategyContainer.RectangleCloneStrategy.GetClone(rectangleShape, cloningMode);
            }
            else if (sourceObject is IEllipseShape ellipseShape)
            {
                return StrategyContainer.EllipseCloneStrategy.GetClone(ellipseShape, cloningMode);
            }
            else if (sourceObject is ICircleShape circleShape)
            {
                return StrategyContainer.CircleCloneStrategy.GetClone(circleShape, cloningMode);
            }
            else if (sourceObject is IRingShape oshape)
            {
                return StrategyContainer.OShapeCloneStrategy.GetClone(oshape, cloningMode);
            }
            else if (sourceObject is ILinePolygonShape linePolygon)
            {
                return StrategyContainer.LinePolygonCloneStrategy.GetClone(linePolygon, cloningMode);
            }
            else if (sourceObject is ITrapezoidShape trapezoidShape)
            {
                return StrategyContainer.TrapezoidShapeCloneStrategy.GetClone(trapezoidShape, cloningMode);
            }
            else if (sourceObject is IVerticalTShape verticalTShape)
            {
                return StrategyContainer.TShapeCloneStrategy.GetClone(verticalTShape, cloningMode);
            }
            else if (sourceObject is IVerticalDoubleTShape verticalDoubleTShape)
            {
                return StrategyContainer.DoubleTShapeCloneStrategy.GetClone(verticalDoubleTShape, cloningMode);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
