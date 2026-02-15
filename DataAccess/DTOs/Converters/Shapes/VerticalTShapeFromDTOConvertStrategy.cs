using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VerticalTShapeFromDTOConvertStrategy : ConvertStrategy<VerticalTShape, VerticalTShapeDTO>
    {
        private IUpdateStrategy<IVerticalTShape> updateStrategy;

        public VerticalTShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IVerticalTShape> UpdateStrategy => updateStrategy ??= new VerticalTShapeUpdateStrategy();

        public override VerticalTShape GetNewItem(VerticalTShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
