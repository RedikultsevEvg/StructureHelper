using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

namespace DataAccess.DTOs
{
    internal class ConcentratedForceFromDTOConvertStrategy : ConvertStrategy<ConcentratedForce, ConcentratedForceDTO>
    {
        private IUpdateStrategy<IConcentratedForce> updateStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationConvertStrategy;

        public ConcentratedForceFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConcentratedForce GetNewItem(ConcentratedForceDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            if (source.ForceValue is not ForceTupleDTO forceTupleDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ForceValue));
            }
            NewItem.ForceValue = tupleConvertStrategy.Convert(forceTupleDTO);
            if (source.CombinationProperty is not FactoredCombinationPropertyDTO combinationPropertyDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.CombinationProperty));
            }
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(combinationPropertyDTO);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ConcentratedForceUpdateStrategy();
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTuple, ForceTupleDTO>
                (this, new ForceTupleFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO>
                (this, new FactoredCombinationPropertyFromDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
