using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

namespace DataAccess.DTOs
{
    internal class DistributedLoadFromDTOConvertStrategy : ConvertStrategy<DistributedLoad, DistributedLoadDTO>
    {
        private IUpdateStrategy<IDistributedLoad> updateStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationConvertStrategy;

        public DistributedLoadFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override DistributedLoad GetNewItem(DistributedLoadDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            if (source.LoadValue is not ForceTupleDTO forceTupleDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.LoadValue));
            }
            NewItem.LoadValue = tupleConvertStrategy.Convert(forceTupleDTO);
            if (source.CombinationProperty is not FactoredCombinationPropertyDTO combinationPropertyDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.CombinationProperty));
            }
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(combinationPropertyDTO);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new DistributedLoadUpdateStrategy();
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTuple, ForceTupleDTO>
                (this, new ForceTupleFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO>
                (this, new FactoredCombinationPropertyFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
