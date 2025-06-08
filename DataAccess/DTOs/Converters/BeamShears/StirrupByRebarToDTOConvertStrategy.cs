using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class StirrupByRebarToDTOConvertStrategy : ConvertStrategy<StirrupByRebarDTO, IStirrupByRebar>
    {
        private StirrupByRebarUpdateStrategy updateStrategy;
        private ReinforcementLibMaterialToDTOConvertStrategy reinforcementConvertStrategy;
        private HelperMaterialDTOSafetyFactorUpdateStrategy safetyFactorUpdateStrategy;

        public StirrupByRebarToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByRebarDTO GetNewItem(IStirrupByRebar source)
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

        private void GetNewStirrup(IStirrupByRebar source)
        {
            TraceLogger?.AddMessage($"Stirrup by density converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Material = reinforcementConvertStrategy.Convert(source.Material);
            safetyFactorUpdateStrategy.Update(NewItem.Material, source.Material);
            TraceLogger?.AddMessage($"Stirrup by density converting Id = {NewItem.Id} has been finished succesfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new StirrupByRebarUpdateStrategy();
            reinforcementConvertStrategy = new ReinforcementLibMaterialToDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            safetyFactorUpdateStrategy = new HelperMaterialDTOSafetyFactorUpdateStrategy(new MaterialSafetyFactorToDTOLogic());
        }
    }
}
