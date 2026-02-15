using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class ShapeToDTOConvertStrategy : ConvertStrategy<IShape, IShape>
    {
        private IConvertStrategy<RectangleShapeDTO, IRectangleShape> rectangleConvertStrategy;
        private IConvertStrategy<EllipseShapeDTO, IEllipseShape> ellipseConvertStrategy;
        private IConvertStrategy<CircleShapeDTO, ICircleShape> circleConvertStrategy;
        private IConvertStrategy<LinePolygonShapeDTO, ILinePolygonShape> linePolygonToDTOConvertStrategy;

        public ShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IShape GetNewItem(IShape source)
        {
            ChildClass = this;
            GetNewShape(source);
            return NewItem;
        }

        private void GetNewShape(IShape source)
        {
            TraceLogger?.AddMessage($"Shape converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            if (source is IRectangleShape rectangle)
            {
                ProcessRectangle(rectangle);
            }
            else if (source is ICircleShape circle)
            {
                ProcessCircle(circle);
            }
            else if (source is IEllipseShape ellipse)
            {
                ProcessEllipse(ellipse);
            }
            else if (source is ILinePolygonShape linePolygon)
            {
                ProcessLinePolygon(linePolygon);
            }
            else if (source is IVerticalTShape tShape)
            {
                ProcessTShape(tShape);
            }
            else if (source is IRingShape ringShape)
            {
                ProcessRingShape(ringShape);
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source) + ": shape type";
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage($"Shape converting Id = {NewItem.Id} has been has been finished successfully", TraceLogStatuses.Debug);
        }

        private void ProcessRingShape(IRingShape ringShape)
        {
            TraceLogger?.AddMessage($"Shape is a ring shape", TraceLogStatuses.Debug);
            var convertStrategy = new DictionaryConvertStrategy<RingShapeDTO, IRingShape>(this, new RingShapeToDTOConvertStrategy(this));
            NewItem = convertStrategy.Convert(ringShape);
        }
        private void ProcessTShape(IVerticalTShape tShape)
        {
            TraceLogger?.AddMessage($"Shape is a vertical T-shape", TraceLogStatuses.Debug);
            var convertStrategy = new DictionaryConvertStrategy<VerticalTShapeDTO, IVerticalTShape>(this, new VerticalTShapeToDTOConvertStrategy(this));
            NewItem = convertStrategy.Convert(tShape);
        }
        private void ProcessEllipse(IEllipseShape ellipse)
        {
            TraceLogger?.AddMessage($"Shape is an ellipse", TraceLogStatuses.Debug);
            ellipseConvertStrategy = new DictionaryConvertStrategy<EllipseShapeDTO, IEllipseShape>
                (this, new EllipseShapeToDTOConvertStrategy(this));
            NewItem = ellipseConvertStrategy.Convert(ellipse);
        }
        private void ProcessLinePolygon(ILinePolygonShape linePolygon)
        {
            TraceLogger?.AddMessage($"Shape is a line polygon", TraceLogStatuses.Debug);
            linePolygonToDTOConvertStrategy = new DictionaryConvertStrategy<LinePolygonShapeDTO, ILinePolygonShape>(this, new LinePolygonToDTOConvertStrategy(this));
            NewItem = linePolygonToDTOConvertStrategy.Convert(linePolygon);
        }
        private void ProcessCircle(ICircleShape circle)
        {
            TraceLogger?.AddMessage($"Shape is a circle", TraceLogStatuses.Debug);
            circleConvertStrategy = new DictionaryConvertStrategy<CircleShapeDTO, ICircleShape>
                (this, new CircleShapeToDTOConvertStrategy(this));
            NewItem = circleConvertStrategy.Convert(circle);
        }
        private void ProcessRectangle(IRectangleShape rectangle)
        {
            TraceLogger?.AddMessage($"Shape is a rectangle", TraceLogStatuses.Debug);
            rectangleConvertStrategy = new DictionaryConvertStrategy<RectangleShapeDTO, IRectangleShape>
                (this, new RectangleShapeToDTOConvertStrategy(this));
            NewItem = rectangleConvertStrategy.Convert(rectangle);
        }
    }
}
