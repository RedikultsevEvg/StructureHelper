using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
     class ElasticFeaMaterialToDTOConvertStrategy : ConvertStrategy<ElasticFeaMaterialDTO, IElasticFeaMaterial>
    {
        private IUpdateStrategy<IElasticFeaMaterial> updateStrategy;

        public ElasticFeaMaterialToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IElasticFeaMaterial> UpdateStrategy => updateStrategy ??= new ElasticFeaMaterialUpdateStrategy();
        public override ElasticFeaMaterialDTO GetNewItem(IElasticFeaMaterial source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
