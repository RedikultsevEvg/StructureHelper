using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    internal interface IShapeCloneStrategyContainer
    {
        ICloneStrategy<ICircleShape> CircleCloneStrategy { get; }
        ICloneStrategy<IVerticalDoubleTShape> DoubleTShapeCloneStrategy { get; }
        ICloneStrategy<IEllipseShape> EllipseCloneStrategy { get; }
        ICloneStrategy<ILinePolygonShape> LinePolygonCloneStrategy { get; }
        ICloneStrategy<IRingShape> OShapeCloneStrategy { get; }
        ICloneStrategy<IRectangleShape> RectangleCloneStrategy { get; }
        ICloneStrategy<ITrapezoidShape> TrapezoidShapeCloneStrategy { get; }
        ICloneStrategy<IVerticalTShape> TShapeCloneStrategy { get; }
    }
}