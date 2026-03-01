using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class FeaMaterialRepositoryToDTOConvertStrategy : ConvertStrategy<FeaMaterialRepositoryDTO, IFeaMaterialRepository>
    {
        private IUpdateStrategy<IFeaMaterialRepository> updateStrategy;
        private IConvertStrategy<ConcreteFeaMaterialDTO, IConcreteFeaMaterial> concreteConvertStrategy;
        private IConvertStrategy<ElasticFeaMaterialDTO, IElasticFeaMaterial> elasticConvertStrategy;

        private IUpdateStrategy<IFeaMaterialRepository> UpdateStrategy => updateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ConcreteFeaMaterialDTO, IConcreteFeaMaterial> ConcreteConvertStrategy => concreteConvertStrategy ??= new ConcreteFeaMaterialToDTOConvertStrategy(this);
        private IConvertStrategy<ElasticFeaMaterialDTO, IElasticFeaMaterial> ElasticConvertStrategy => elasticConvertStrategy ??= new ElasticFeaMaterialToDTOConvertStrategy(this);
        public FeaMaterialRepositoryToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

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
            else if (material is IConcreteFeaMaterial concreteFea)
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
