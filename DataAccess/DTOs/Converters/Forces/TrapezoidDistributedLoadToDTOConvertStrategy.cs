using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    internal class TrapezoidDistributedLoadToDTOConvertStrategy : ConvertStrategy<TrapezoidDistributedLoadDTO, ITrapezoidDistributedLoad>
    {
        private IUpdateStrategy<ITrapezoidDistributedLoad> updateStrategy;
        private IUpdateStrategy<ITrapezoidDistributedLoad> UpdateStrategy => updateStrategy ??= new TrapezoidDistributedLoadUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ForceTupleDTO, IForceTuple> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationConvertStrategy;

        public TrapezoidDistributedLoadToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override TrapezoidDistributedLoadDTO GetNewItem(ITrapezoidDistributedLoad source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            UpdateStrategy.Update(NewItem, source);
            NewItem.StartLoadValue = tupleConvertStrategy.Convert(source.StartLoadValue);
            NewItem.EndLoadValue = tupleConvertStrategy.Convert(source.EndLoadValue);
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(source.CombinationProperty);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>
                (this, new ForceTupleToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>
                (this, new FactoredCombinationPropertyToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
