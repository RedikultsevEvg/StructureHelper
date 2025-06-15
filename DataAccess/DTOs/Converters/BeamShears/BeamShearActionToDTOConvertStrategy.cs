using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;

namespace DataAccess.DTOs
{
    public class BeamShearActionToDTOConvertStrategy : ConvertStrategy<BeamShearActionDTO, IBeamShearAction>
    {
        private IUpdateStrategy<IBeamShearAction> updateStrategy;
        private IConvertStrategy<FactoredForceTupleDTO, IFactoredForceTuple> factoredTupleConvertStrategy;
        private IConvertStrategy<BeamShearAxisActionDTO, IBeamShearAxisAction> axisActionConvertStrategy;

        public BeamShearActionToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public BeamShearActionToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearActionDTO GetNewItem(IBeamShearAction source)
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

        private void GetNewBeamAction(IBeamShearAction source)
        {
            TraceLogger?.AddMessage($"Beam shear action converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.ExternalForce = factoredTupleConvertStrategy.Convert(source.ExternalForce);
            NewItem.SupportAction = axisActionConvertStrategy.Convert(source.SupportAction);
            TraceLogger?.AddMessage($"Beam shear action converting Id = {NewItem.Id} has been finished", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearActionUpdateStrategy();
            factoredTupleConvertStrategy = new DictionaryConvertStrategy<FactoredForceTupleDTO, IFactoredForceTuple>
                (this, new FactoredForceTupleToDTOConvertStrategy(this));
            axisActionConvertStrategy = new DictionaryConvertStrategy<BeamShearAxisActionDTO, IBeamShearAxisAction>
                (this, new BeamShearAxisActionToDTOConvertStrategy(this));
        }
    }
}
