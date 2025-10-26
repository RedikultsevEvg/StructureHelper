using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class VisualPropertyFromDTOConvertStrategy : ConvertStrategy<IVisualProperty, VisualPropertyDTO>
    {
        IUpdateStrategy<IVisualProperty> updateStrategy;

        public VisualPropertyFromDTOConvertStrategy(IUpdateStrategy<IVisualProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public VisualPropertyFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new VisualPropsUpdateStrategy();
        }

        public override IVisualProperty GetNewItem(VisualPropertyDTO source)
        {
            NewItem = new VisualProperty(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
