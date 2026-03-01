using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ConcreteFeaCompressionFromDTOConvertStrategy : ConvertStrategy<ConcreteFeaCompression, ConcreteFeaCompressionDTO>
    {
        private IUpdateStrategy<IConcreteFeaCompression> updateStrategy;

        public ConcreteFeaCompressionFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaCompression> UpdateStrategy => updateStrategy ??= new ConcreteFeaCompressionUpdateStrategy();
        public override ConcreteFeaCompression GetNewItem(ConcreteFeaCompressionDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
