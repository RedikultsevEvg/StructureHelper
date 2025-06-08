using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearAxisActionToDTOConvertStrategy : ConvertStrategy<BeamShearAxisActionDTO, IBeamShearAxisAction>
    {
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy;
        private IConvertStrategy<FactoredForceTupleDTO, IFactoredForceTuple> factoredTupleConvertStrategy;
        private IConvertStrategy<IBeamSpanLoad, IBeamSpanLoad> spanLoadConvertStrategy;

        public BeamShearAxisActionToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearAxisActionDTO GetNewItem(IBeamShearAxisAction source)
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

        private void GetNewBeamAction(IBeamShearAxisAction source)
        {
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.SupportForce = factoredTupleConvertStrategy.Convert(source.SupportForce);
            NewItem.ShearLoads.Clear();
            foreach (var spanLoad in source.ShearLoads)
            {
                var newSpanLoad = spanLoadConvertStrategy.Convert(spanLoad);
                NewItem.ShearLoads.Add(newSpanLoad);
            }
            TraceLogger?.AddMessage($"Converting of beam shear action Id = {source.Id} has been finished", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
            factoredTupleConvertStrategy = new DictionaryConvertStrategy<FactoredForceTupleDTO, IFactoredForceTuple>
                (this, new FactoredForceTupleToDTOConvertStrategy(this));
            spanLoadConvertStrategy = new DictionaryConvertStrategy<IBeamSpanLoad, IBeamSpanLoad>
                (this, new BeamSpanLoadToDTOConvertStrategy(this));
        }
    }
}
