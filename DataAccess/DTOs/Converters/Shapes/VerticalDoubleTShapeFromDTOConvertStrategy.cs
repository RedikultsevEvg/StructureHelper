using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class VerticalDoubleTShapeFromDTOConvertStrategy : ConvertStrategy<VerticalDoubleTShape, VerticalDoubleTShapeDTO>
    {
        private IUpdateStrategy<IVerticalDoubleTShape> updateStrategy;

        public VerticalDoubleTShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IVerticalDoubleTShape> UpdateStrategy => updateStrategy ??= new VerticalDoubleTShapeUpdateStrategy();

        public override VerticalDoubleTShape GetNewItem(VerticalDoubleTShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
