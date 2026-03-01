using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ConcreteFeaCompressionToDTOConvertStrategy : ConvertStrategy<ConcreteFeaCompressionDTO, IConcreteFeaCompression>
    {
        private IUpdateStrategy<IConcreteFeaCompression> updateStrategy;

        public ConcreteFeaCompressionToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaCompression> UpdateStrategy => updateStrategy ??= new ConcreteFeaCompressionUpdateStrategy();
        public override ConcreteFeaCompressionDTO GetNewItem(IConcreteFeaCompression source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
