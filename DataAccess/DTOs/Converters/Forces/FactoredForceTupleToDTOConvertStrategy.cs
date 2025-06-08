using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class FactoredForceTupleToDTOConvertStrategy : ConvertStrategy<FactoredForceTupleDTO, IFactoredForceTuple>
    {
        private IConvertStrategy<ForceTupleDTO, IForceTuple> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationConvertStrategy;

        public FactoredForceTupleToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override FactoredForceTupleDTO GetNewItem(IFactoredForceTuple source)
        {
            try
            {
                GetNewFactoredForceTuple(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewFactoredForceTuple(IFactoredForceTuple source)
        {
            TraceLogger?.AddMessage($"Converting of factored force tuple Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            NewItem.ForceTuple = tupleConvertStrategy.Convert(source.ForceTuple);
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(source.CombinationProperty);
            TraceLogger?.AddMessage($"Converting of factored force tuple Id = {NewItem.Id} has been finished", TraceLogStatuses.Debug);
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
