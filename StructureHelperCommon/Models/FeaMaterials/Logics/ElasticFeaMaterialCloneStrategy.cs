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

        public IElasticFeaMaterial GetClone(IElasticFeaMaterial sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            ElasticFeaMaterial clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
