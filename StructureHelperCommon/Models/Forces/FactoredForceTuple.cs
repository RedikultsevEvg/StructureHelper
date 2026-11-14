using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    public class FactoredForceTuple : IFactoredForceTuple
    {
        private IUpdateStrategy<IFactoredForceTuple> updateStrategy;

        public Guid Id { get; }
        public IForceTuple ForceTuple { get; set; } = new ForceTuple();
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid());

        public FactoredForceTuple(Guid id)
        {
            Id = id;
        }

        public IFactoredForceTuple GetClone(IFactoredForceTuple sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            FactoredForceTuple newItem = new(id);
            updateStrategy ??= new FactoredForceTupleUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
