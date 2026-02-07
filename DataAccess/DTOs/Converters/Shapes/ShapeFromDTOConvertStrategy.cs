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
                var polygonConvertStrategy = new DictionaryConvertStrategy<ILinePolygonShape, ILinePolygonShape>
                    (this, new LinePolygonFromDTOConvertStrategy(this));
                NewItem = polygonConvertStrategy.Convert(linePolygonDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": shape is unknown");
            }
            return NewItem;
        }
    }
}
