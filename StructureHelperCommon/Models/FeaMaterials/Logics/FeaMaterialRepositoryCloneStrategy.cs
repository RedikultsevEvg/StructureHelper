using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialRepositoryCloneStrategy : ICloneStrategy<IFeaMaterialRepository>
    {
        private IUpdateStrategy<IFeaMaterialRepository> updateStrategy;
        private IUpdateStrategy<IFeaMaterialRepository> UpdateStrategy => updateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = true};

        public IFeaMaterialRepository GetClone(IFeaMaterialRepository sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, "FEA material repository");
            FeaMaterialRepository clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
