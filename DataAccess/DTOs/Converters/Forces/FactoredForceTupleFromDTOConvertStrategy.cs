using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class FactoredForceTupleFromDTOConvertStrategy : ConvertStrategy<FactoredForceTuple, FactoredForceTupleDTO>
    {
        private IConvertStrategy<ForceTuple, ForceTupleDTO> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationProperty, FactoredCombinationPropertyDTO> combinationConvertStrategy;

        public FactoredForceTupleFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy)
            : base(baseConvertStrategy)
        {
        }

        public override FactoredForceTuple GetNewItem(FactoredForceTupleDTO source)
        {
            ChildClass = this;
            CheckObjects(source);
            InitializeStrategies();
            return GetNewFactoredTuple(source);
        }

        private FactoredForceTuple GetNewFactoredTuple(FactoredForceTupleDTO source)
        {
            if (source.ForceTuple is not ForceTupleDTO forceTupleDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ForceTuple));
            }
            if (source.CombinationProperty is not FactoredCombinationPropertyDTO combinationPropertyDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ForceTuple));
            }
            NewItem = new(source.Id)
            {
                ForceTuple = tupleConvertStrategy.Convert(forceTupleDTO),
                CombinationProperty = combinationConvertStrategy.Convert(combinationPropertyDTO)
            };
            return NewItem;
        }

        private static void CheckObjects(FactoredForceTupleDTO source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.ForceTuple);
            CheckObject.ThrowIfNull(source.CombinationProperty);
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
