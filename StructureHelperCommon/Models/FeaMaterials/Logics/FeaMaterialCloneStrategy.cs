using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialCloneStrategy : ICloneStrategy<IFeaMaterial>
    {
        private ICloneStrategy<IConcreteFeaMaterial> concreteCloneStrategy;
        private ICloneStrategy<IElasticFeaMaterial> elasticCloneStrategy;
        private ICloneStrategy<IConcreteFeaMaterial> ConcreteCloneStrategy => concreteCloneStrategy ??= new ConcreteFeaMaterialCloneStrategy();
        private ICloneStrategy<IElasticFeaMaterial> ElasticCloneStrategy => elasticCloneStrategy ??= new ElasticFeaMaterialCloneStrategy();

        public IFeaMaterial GetClone(IFeaMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            if (sourceObject is IElasticFeaMaterial elasticFeaMaterial)
            {
                return ElasticCloneStrategy.GetClone(elasticFeaMaterial, cloneMode);
            }
            else if (sourceObject is IConcreteFeaMaterial concreteFea)
            {
                return ConcreteCloneStrategy.GetClone(concreteFea, cloneMode);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject) + ": FEA material");
            }
        }
    }
}
