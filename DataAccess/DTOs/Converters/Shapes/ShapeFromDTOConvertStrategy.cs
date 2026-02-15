using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class ShapeFromDTOConvertStrategy : ConvertStrategy<IShape, IShape>
    {
        private IConvertStrategy<RectangleShape, RectangleShapeDTO> rectangleConvertStrategy;
        private IConvertStrategy<EllipseShape, EllipseShapeDTO> ellipseConvertStrategy;
        private IConvertStrategy<EllipseShape, EllipseShapeDTO> EllipseConvertStrategy =>
            ellipseConvertStrategy ??= 
            new DictionaryConvertStrategy<EllipseShape, EllipseShapeDTO>(this, new EllipseShapeFromDTOConvertStrategy(this));
        private IConvertStrategy<CircleShape, CircleShapeDTO> circleConvertStrategy;

        public ShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IShape GetNewItem(IShape source)
        {
            ChildClass = this;
            if (source is RectangleShapeDTO rectangleShapeDTO)
            {
                rectangleConvertStrategy ??= new DictionaryConvertStrategy<RectangleShape, RectangleShapeDTO>
                    (this, new RectangleShapeFromDTOConvertStrategy(this));
                NewItem = rectangleConvertStrategy.Convert(rectangleShapeDTO);
            }
            else if (source is CircleShapeDTO circleShapeDTO)
            {
                circleConvertStrategy ??= new DictionaryConvertStrategy<CircleShape, CircleShapeDTO>
                    (this, new CircleShapeFromDTOConvertStrategy(this));
                NewItem = circleConvertStrategy.Convert(circleShapeDTO);
            }
            else if (source is EllipseShapeDTO ellipseShapeDTO)
            {
                NewItem = EllipseConvertStrategy.Convert(ellipseShapeDTO);
            }
            else if (source is LinePolygonShapeDTO linePolygonDTO)
            {
                TraceLogger?.AddMessage("Shape is a line polygon shape");
                var polygonConvertStrategy = new DictionaryConvertStrategy<ILinePolygonShape, ILinePolygonShape>(this, new LinePolygonFromDTOConvertStrategy(this));
                NewItem = polygonConvertStrategy.Convert(linePolygonDTO);
            }
            else if (source is VerticalTShapeDTO tShapeDTO)
            {
                TraceLogger?.AddMessage("Shape is a vertical t-shape");
                var strategy = new DictionaryConvertStrategy<VerticalTShape, VerticalTShapeDTO>(this, new VerticalTShapeFromDTOConvertStrategy(this));
                NewItem = strategy.Convert(tShapeDTO);
            }
            else if (source is RingShapeDTO ringShapeDTO)
            {
                TraceLogger?.AddMessage("Shape is a ring shape");
                var strategy = new DictionaryConvertStrategy<RingShape, RingShapeDTO>(this, new RingShapeFromDTOConvertStrategy(this));
                NewItem = strategy.Convert(ringShapeDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": shape is unknown");
            }
            return NewItem;
        }
    }
}
