using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialCloneStrategy : ICloneStrategy<IFeaMaterial>
    {
        private ICloneStrategy<IElasticFeaMaterial> elasticCloneStrategy;
        private ICloneStrategy<IElasticFeaMaterial> ElasticCloneStrategy => elasticCloneStrategy ??= new ElasticFeaMaterialCloneStrategy();
        public IFeaMaterial GetClone(IFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (sourceObject is IElasticFeaMaterial elasticFeaMaterial)
            {
                return ElasticCloneStrategy.GetClone(elasticFeaMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject) + ": FEA material");
            }
        }
    }
}
