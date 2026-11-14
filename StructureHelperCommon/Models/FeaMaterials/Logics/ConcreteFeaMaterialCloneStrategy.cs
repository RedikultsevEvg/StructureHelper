using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaMaterialCloneStrategy : ICloneStrategy<IConcreteFeaMaterial>
    {
        private IUpdateStrategy<IConcreteFeaMaterial> updateStrategy;
        private IUpdateStrategy<IConcreteFeaMaterial> UpdateStrategy => updateStrategy ??= new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = true };

        public IConcreteFeaMaterial GetClone(IConcreteFeaMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            ConcreteFeaMaterial clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
