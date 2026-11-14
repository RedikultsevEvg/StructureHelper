using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticFeaMaterialCloneStrategy : ICloneStrategy<IElasticFeaMaterial>
    {
        private IUpdateStrategy<IElasticFeaMaterial> updateStrategy;
        private IUpdateStrategy<IElasticFeaMaterial> UpdateStrategy => updateStrategy ??= new ElasticFeaMaterialUpdateStrategy();

        public IElasticFeaMaterial GetClone(IElasticFeaMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            ElasticFeaMaterial clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
