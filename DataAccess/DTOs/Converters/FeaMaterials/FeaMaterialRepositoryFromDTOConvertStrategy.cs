using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class FeaMaterialRepositoryFromDTOConvertStrategy : ConvertStrategy<FeaMaterialRepository, FeaMaterialRepositoryDTO>
    {
        private IUpdateStrategy<IFeaMaterialRepository> updateStrategy;
        private IConvertStrategy<ConcreteFeaMaterial, ConcreteFeaMaterialDTO> concreteConvertStrategy;
        private IConvertStrategy<ElasticFeaMaterial, ElasticFeaMaterialDTO> elasticConvertStrategy;

        public FeaMaterialRepositoryFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IFeaMaterialRepository> UpdateStrategy => updateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ConcreteFeaMaterial, ConcreteFeaMaterialDTO> ConcreteConvertStrategy => concreteConvertStrategy ??= new ConcreteFeaMaterialFromDTOConvertStrategy(this);
        private IConvertStrategy<ElasticFeaMaterial, ElasticFeaMaterialDTO> ElasticConvertStrategy => elasticConvertStrategy ??= new ElasticFeaMaterialFromDTOConvertStrategy(this);

        public override FeaMaterialRepository GetNewItem(FeaMaterialRepositoryDTO source)
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
            if (material is ElasticFeaMaterialDTO elasticFeaMaterial)
            {
                NewItem.FeaMaterials.Add(ElasticConvertStrategy.Convert(elasticFeaMaterial));
            }
            else if (material is ConcreteFeaMaterialDTO concreteFea)
            {
                NewItem.FeaMaterials.Add(ConcreteConvertStrategy.Convert(concreteFea));
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
        }

    }
}
