using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ConcreteFeaTensionToDTOConvertStrategy : ConvertStrategy<ConcreteFeaTensionDTO, IConcreteFeaTension>
    {
        private IUpdateStrategy<IConcreteFeaTension> updateStrategy;

        public ConcreteFeaTensionToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IConcreteFeaTension> UpdateStrategy => updateStrategy ??= new ConcreteFeaTensionUpdateStrategy();
        public override ConcreteFeaTensionDTO GetNewItem(IConcreteFeaTension source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
