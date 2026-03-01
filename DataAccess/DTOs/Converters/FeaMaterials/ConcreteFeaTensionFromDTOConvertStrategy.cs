using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ConcreteFeaTensionFromDTOConvertStrategy : ConvertStrategy<ConcreteFeaTension, ConcreteFeaTensionDTO>
    {
        private IUpdateStrategy<IConcreteFeaTension> updateStrategy;

        public ConcreteFeaTensionFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaTension> UpdateStrategy => updateStrategy ??= new ConcreteFeaTensionUpdateStrategy();
        public override ConcreteFeaTension GetNewItem(ConcreteFeaTensionDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
