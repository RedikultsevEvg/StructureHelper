using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    internal class ShapeCloneStrategyContainer : IShapeCloneStrategyContainer
    {
        private ICloneStrategy<ICircleShape> circleCloneStrategy;
        private ICloneStrategy<IVerticalDoubleTShape> doubleTShapeCloneStrategy;
        private ICloneStrategy<IEllipseShape> ellipseCloneStrategy;
        private ICloneStrategy<ILinePolygonShape> linePolygonCloneStrategy;
        private ICloneStrategy<IRingShape> oShapeCloneStrategy;
        private ICloneStrategy<IRectangleShape> rectangleCloneStrategy;
        private ICloneStrategy<ITrapezoidShape> trapezoidShapeCloneStrategy;
        private ICloneStrategy<IVerticalTShape> tShapeCloneStrategy;

        public ICloneStrategy<ICircleShape> CircleCloneStrategy => circleCloneStrategy ??= new CircleShapeCloneStrategy();
        public ICloneStrategy<IVerticalDoubleTShape> DoubleTShapeCloneStrategy => doubleTShapeCloneStrategy ??= new VerticalDoubleTShapeCloneStrategy();
        public ICloneStrategy<IEllipseShape> EllipseCloneStrategy => ellipseCloneStrategy ??= new EllipseShapeCloneStrategy();
        public ICloneStrategy<ILinePolygonShape> LinePolygonCloneStrategy => linePolygonCloneStrategy ??= new LinePolygonShapeCloneStrategy();
        public ICloneStrategy<IRingShape> OShapeCloneStrategy => oShapeCloneStrategy ??= new RingShapeCloneStrategy();
        public ICloneStrategy<IRectangleShape> RectangleCloneStrategy => rectangleCloneStrategy ??= new RectangleShapeCloneStrategy();
        public ICloneStrategy<ITrapezoidShape> TrapezoidShapeCloneStrategy => trapezoidShapeCloneStrategy ??= new TrapezoidShapeCloneStrategy();
        public ICloneStrategy<IVerticalTShape> TShapeCloneStrategy => tShapeCloneStrategy ??= new VerticalTShapeCloneStrategy();
    }
}
