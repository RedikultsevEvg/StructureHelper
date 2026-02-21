using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class TrapezoidShapeToDTOConvertStrategy : ConvertStrategy<TrapezoidShapeDTO, ITrapezoidShape>
    {
        private IUpdateStrategy<ITrapezoidShape> updateStrategy;

        public TrapezoidShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<ITrapezoidShape> UpdateStrategy => updateStrategy ??= new TrapezoidShapeUpdateStrategy();

        public override TrapezoidShapeDTO GetNewItem(ITrapezoidShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
