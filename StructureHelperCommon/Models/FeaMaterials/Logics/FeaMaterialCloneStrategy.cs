using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialCloneStrategy : ICloneStrategy<IFeaMaterial>
    {
        private ICloneStrategy<IConcreteFeaMaterial> concreteCloneStrategy;
        private ICloneStrategy<IElasticFeaMaterial> elasticCloneStrategy;
        private ICloneStrategy<IConcreteFeaMaterial> ConcreteCloneStrategy => concreteCloneStrategy ??= new ConcreteFeaMaterialCloneStrategy();
        private ICloneStrategy<IElasticFeaMaterial> ElasticCloneStrategy => elasticCloneStrategy ??= new ElasticFeaMaterialCloneStrategy();
        public IFeaMaterial GetClone(IFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (sourceObject is IElasticFeaMaterial elasticFeaMaterial)
            {
                return ElasticCloneStrategy.GetClone(elasticFeaMaterial);
            }
            else if (sourceObject is IConcreteFeaMaterial concreteFea)
            {
                return ConcreteCloneStrategy.GetClone(concreteFea);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject) + ": FEA material");
            }
        }
    }
}
