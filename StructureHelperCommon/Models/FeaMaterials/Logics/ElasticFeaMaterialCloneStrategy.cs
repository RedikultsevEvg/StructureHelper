using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticFeaMaterialCloneStrategy : ICloneStrategy<IElasticFeaMaterial>
    {
        private IUpdateStrategy<IElasticFeaMaterial> updateStrategy;
        private IUpdateStrategy<IElasticFeaMaterial> UpdateStrategy => updateStrategy ??= new ElasticFeaMaterialUpdateStrategy();
        public IElasticFeaMaterial GetClone(IElasticFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            ElasticFeaMaterial clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
