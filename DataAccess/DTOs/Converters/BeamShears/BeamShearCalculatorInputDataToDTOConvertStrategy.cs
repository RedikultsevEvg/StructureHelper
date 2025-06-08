using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<BeamShearCalculatorInputDataDTO, IBeamShearCalculatorInputData>
    {
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;

        public BeamShearCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearCalculatorInputDataDTO GetNewItem(IBeamShearCalculatorInputData source)
        {
            try
            {
                GetNewInputData(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewInputData(IBeamShearCalculatorInputData source)
        {
            TraceLogger?.AddMessage($"Input data converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            TraceLogger?.AddMessage($"Input data converting Id = {NewItem.Id} has been finished", TraceLogStatuses.Debug);

        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasBeamShearActionToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupToDTOConvertStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
