using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ForceTupleToDTOConvertStrategy : ConvertStrategy<ForceTupleDTO, IForceTuple>
    {
        private IUpdateStrategy<IForceTuple> updateStrategy;

        public ForceTupleToDTOConvertStrategy(IUpdateStrategy<IForceTuple> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ForceTupleToDTOConvertStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }

        public ForceTupleToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override ForceTupleDTO GetNewItem(IForceTuple source)
        {
            try
            {
                GetNewBeamForceTuple(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }

        }

        private void GetNewBeamForceTuple(IForceTuple source)
        {
            TraceLogger?.AddMessage($"Converting of force tuple Id = {source.Id} has been started", TraceLogStatuses.Debug);
            updateStrategy ??= new ForceTupleUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage($"Converting of force tuple Id = {source.Id} has been finished", TraceLogStatuses.Debug);
        }
    }
}
