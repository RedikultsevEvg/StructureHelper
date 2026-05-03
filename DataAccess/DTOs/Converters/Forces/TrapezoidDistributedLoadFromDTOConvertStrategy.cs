using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    internal class TrapezoidDistributedLoadFromDTOConvertStrategy : ConvertStrategy<TrapezoidDistributedLoad, TrapezoidDistributedLoadDTO>
    {
        private IUpdateStrategy<ITrapezoidDistributedLoad> updateStrategy;
        private IUpdateStrategy<ITrapezoidDistributedLoad> UpdateStrategy => updateStrategy ??= new TrapezoidDistributedLoadUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ForceTuple, ForceTupleDTO> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationConvertStrategy;

        public TrapezoidDistributedLoadFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override TrapezoidDistributedLoad GetNewItem(TrapezoidDistributedLoadDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            UpdateStrategy.Update(NewItem, source);
            if (source.StartLoadValue is not ForceTupleDTO startLoadValue)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.StartLoadValue));
            }
            if (source.EndLoadValue is not ForceTupleDTO endLoadValue)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.EndLoadValue));
            }
            if (source.CombinationProperty is not FactoredCombinationPropertyDTO combinationProperty)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.CombinationProperty));
            }
            NewItem.StartLoadValue = tupleConvertStrategy.Convert(startLoadValue);
            NewItem.EndLoadValue = tupleConvertStrategy.Convert(endLoadValue);
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(combinationProperty);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTuple, ForceTupleDTO>
                (this, new ForceTupleFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO>
                (this, new FactoredCombinationPropertyFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
