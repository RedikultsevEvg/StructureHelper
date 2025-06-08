using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class DistributedLoadToDTOConvertStrategy : ConvertStrategy<DistributedLoadDTO, IDistributedLoad>
    {
        private IUpdateStrategy<IDistributedLoad> updateStrategy;
        private IConvertStrategy<ForceTupleDTO, IForceTuple> tupleConvertStrategy;
        private IConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty> combinationConvertStrategy;

        public DistributedLoadToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override DistributedLoadDTO GetNewItem(IDistributedLoad source)
        {
            try
            {
                GetNewBeamAction(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewBeamAction(IDistributedLoad source)
        {
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.LoadValue = tupleConvertStrategy.Convert(source.LoadValue);
            NewItem.CombinationProperty = combinationConvertStrategy.Convert(source.CombinationProperty);
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been finished", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new DistributedLoadUpdateStrategy();
            tupleConvertStrategy = new DictionaryConvertStrategy<ForceTupleDTO, IForceTuple>
                (this, new ForceTupleToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
            combinationConvertStrategy = new DictionaryConvertStrategy<FactoredCombinationPropertyDTO, IFactoredCombinationProperty>
                (this, new FactoredCombinationPropertyToDTOConvertStrategy(ReferenceDictionary, TraceLogger));
        }
    }
}
