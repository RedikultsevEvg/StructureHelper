using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaMaterialCloneStrategy : ICloneStrategy<IConcreteFeaMaterial>
    {
        private IUpdateStrategy<IConcreteFeaMaterial> updateStrategy;
        private IUpdateStrategy<IConcreteFeaMaterial> UpdateStrategy => updateStrategy ??= new ConcreteFeaMaterialUpdateStrategy() { UpdateChildren = true };
        public IConcreteFeaMaterial GetClone(IConcreteFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            ConcreteFeaMaterial clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
