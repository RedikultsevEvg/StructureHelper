using StructureHelperCommon.Infrastructures.Interfaces;
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

        public object Clone()
        {
            updateStrategy ??= new FactoredForceTupleUpdateStrategy();
            FactoredForceTuple newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
