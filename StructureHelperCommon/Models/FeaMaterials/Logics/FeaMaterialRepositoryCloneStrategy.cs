using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialRepositoryCloneStrategy : ICloneStrategy<IFeaMaterialRepository>
    {
        private IUpdateStrategy<IFeaMaterialRepository> updateStrategy;
        private IUpdateStrategy<IFeaMaterialRepository> UpdateStrategy => updateStrategy ??= new FeaMaterialRepositoryUpdateStrategy() { UpdateChildren = true};

        public IFeaMaterialRepository GetClone(IFeaMaterialRepository sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject, "FEA material repository");
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            FeaMaterialRepository clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
