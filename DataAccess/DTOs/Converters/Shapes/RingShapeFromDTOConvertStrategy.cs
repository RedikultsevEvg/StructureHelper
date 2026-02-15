using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class RingShapeFromDTOConvertStrategy : ConvertStrategy<RingShape, RingShapeDTO>
    {
        private IUpdateStrategy<IRingShape> updateStrategy;

        public RingShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IRingShape> UpdateStrategy => updateStrategy ??= new RingShapeUpdateStrategy();
        public override RingShape GetNewItem(RingShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
