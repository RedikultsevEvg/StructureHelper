using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class ShapeFromDTOConvertStrategy : ConvertStrategy<IShape, IShape>
    {
        private IConvertStrategy<RectangleShape, RectangleShapeDTO> rectangleConvertStrategy;

        public ShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override IShape GetNewItem(IShape source)
        {
            if (source is RectangleShapeDTO rectangleShapeDTO)
            {
                rectangleConvertStrategy ??= new DictionaryConvertStrategy<RectangleShape, RectangleShapeDTO>
                    (this, new RectangleShapeFromDTOConvertStrategy(this));
                NewItem = rectangleConvertStrategy.Convert(rectangleShapeDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": shape is unknown");
            }
            return NewItem;
        }
    }
}
