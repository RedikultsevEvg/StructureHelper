using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

namespace DataAccess.DTOs
{
    public class ConcentratedForceToDTOConvertStrategy : ConvertStrategy<ConcentratedForceDTO, IConcentratedForce>
    {
        private IUpdateStrategy<IConcentratedForce> updateStrategy;
        private IConvertStrategy<ForceTupleDTO, IForceTuple> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationConvertStrategy;

        public ConcentratedForceToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConcentratedForceDTO GetNewItem(IConcentratedForce source)
        {
            try
            {
                GetNewConcentratedForce(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewConcentratedForce(IConcentratedForce source)
        {
            TraceLogger?.AddMessage($"Concentrated force converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.ForceValue = tupleConvertStrategy.Convert(source.ForceValue);
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(source.CombinationProperty);
            TraceLogger?.AddMessage($"Concentrated force converting Id = {NewItem.Id} has been finished successfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ConcentratedForceUpdateStrategy();
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>
                (this, new ForceTupleToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>
                (this, new FactoredCombinationPropertyToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
