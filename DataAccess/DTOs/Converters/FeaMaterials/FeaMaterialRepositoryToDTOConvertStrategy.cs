using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class FeaMaterialRepositoryToDTOConvertStrategy : ConvertStrategy<FeaMaterialRepositoryDTO, IFeaMaterialRepository>
    {
        private IUpdateStrategy<IFeaMaterialRepository> updateStrategy;
        private IConvertStrategy<ElasticFeaMaterialDTO, IElasticFeaMaterial> elasticConvertStrategy;

        public FeaMaterialRepositoryToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IFeaMaterialRepository> UpdateStrategy => updateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ElasticFeaMaterialDTO, IElasticFeaMaterial> ElasticConvertStrategy => elasticConvertStrategy ??= new ElasticFeaMaterialToDTOConvertStrategy(this);

        public override FeaMaterialRepositoryDTO GetNewItem(IFeaMaterialRepository source)
        {
            ChildClass = this;
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.FeaMaterials);
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            foreach (var material in source.FeaMaterials)
            {
                ProcessMaterial(material);
            }
            return NewItem;
        }

        private void ProcessMaterial(IFeaMaterial material)
        {
            if (material is IElasticFeaMaterial elasticFeaMaterial)
            {
                NewItem.FeaMaterials.Add(ElasticConvertStrategy.Convert(elasticFeaMaterial));
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
        }
    }
}
