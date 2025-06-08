using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class StirrupByDensityToDTOConvertStrategy : ConvertStrategy<StirrupByDensityDTO, IStirrupByDensity>
    {
        private IUpdateStrategy<IStirrupByDensity> updateStrategy;

        public StirrupByDensityToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByDensityDTO GetNewItem(IStirrupByDensity source)
        {
            try
            {
                GetNewStirrup(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewStirrup(IStirrupByDensity source)
        {
            TraceLogger?.AddMessage($"Stirrup by density converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage($"Stirrup by density converting Id = {NewItem.Id} has been finished succesfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new StirrupByDensityUpdateStrategy();
        }
    }
}
