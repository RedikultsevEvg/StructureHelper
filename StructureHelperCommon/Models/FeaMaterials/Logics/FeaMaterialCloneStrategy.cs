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

        public IFeaMaterial GetClone(IFeaMaterial sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            if (sourceObject is IElasticFeaMaterial elasticFeaMaterial)
            {
                return ElasticCloneStrategy.GetClone(elasticFeaMaterial, cloningMode);
            }
            else if (sourceObject is IConcreteFeaMaterial concreteFea)
            {
                return ConcreteCloneStrategy.GetClone(concreteFea, cloningMode);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject) + ": FEA material");
            }
        }
    }
}
