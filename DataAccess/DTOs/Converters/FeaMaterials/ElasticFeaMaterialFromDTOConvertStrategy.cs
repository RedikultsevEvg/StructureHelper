using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class ElasticFeaMaterialFromDTOConvertStrategy : ConvertStrategy<ElasticFeaMaterial, ElasticFeaMaterialDTO>
    {
        private IUpdateStrategy<IElasticFeaMaterial> updateStrategy;

        public ElasticFeaMaterialFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IElasticFeaMaterial> UpdateStrategy => updateStrategy ??= new ElasticFeaMaterialUpdateStrategy();
        public override ElasticFeaMaterial GetNewItem(ElasticFeaMaterialDTO source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
