using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class RectangleShapeFromDTOConvertStrategy : ConvertStrategy<RectangleShape, RectangleShapeDTO>
    {
        private IUpdateStrategy<IRectangleShape> updateStrategy;
        private IUpdateStrategy<IRectangleShape> UpdateStrategy => updateStrategy ??= new RectangleShapeUpdateStrategy();

        public RectangleShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override RectangleShape GetNewItem(RectangleShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
