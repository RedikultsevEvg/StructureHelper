using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class RingShapeToDTOConvertStrategy : ConvertStrategy<RingShapeDTO, IRingShape>
    {
        private IUpdateStrategy<IRingShape> updateStrategy;

        public RingShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IRingShape> UpdateStrategy => updateStrategy ??= new RingShapeUpdateStrategy();
        public override RingShapeDTO GetNewItem(IRingShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
