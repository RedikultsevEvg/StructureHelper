using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class VerticalDoubleTShapeToDTOConvertStrategy : ConvertStrategy<VerticalDoubleTShapeDTO, IVerticalDoubleTShape>
    {
        private IUpdateStrategy<IVerticalDoubleTShape> updateStrategy;

        public VerticalDoubleTShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IVerticalDoubleTShape> UpdateStrategy => updateStrategy ??= new VerticalDoubleTShapeUpdateStrategy();

        public override VerticalDoubleTShapeDTO GetNewItem(IVerticalDoubleTShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
