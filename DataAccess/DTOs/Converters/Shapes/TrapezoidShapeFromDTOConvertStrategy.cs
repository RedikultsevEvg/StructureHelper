using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class TrapezoidShapeFromDTOConvertStrategy : ConvertStrategy<TrapezoidShape, TrapezoidShapeDTO>
    {
        private IUpdateStrategy<ITrapezoidShape> updateStrategy;

        public TrapezoidShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<ITrapezoidShape> UpdateStrategy => updateStrategy ??= new TrapezoidShapeUpdateStrategy();

        public override TrapezoidShape GetNewItem(TrapezoidShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
