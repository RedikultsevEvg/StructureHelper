using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VerticalTShapeToDTOConvertStrategy : ConvertStrategy<VerticalTShapeDTO, IVerticalTShape>
    {
        private IUpdateStrategy<IVerticalTShape> updateStrategy;
        private IUpdateStrategy<IVerticalTShape> UpdateStrategy => updateStrategy ??= new VerticalTShapeUpdateStrategy();

        public VerticalTShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }
        public override VerticalTShapeDTO GetNewItem(IVerticalTShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }


    }
}
