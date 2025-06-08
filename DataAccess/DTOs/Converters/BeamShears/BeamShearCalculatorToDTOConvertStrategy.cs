using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearCalculatorToDTOConvertStrategy : ConvertStrategy<BeamShearCalculatorDTO, IBeamShearCalculator>
    {
        private IUpdateStrategy<IBeamShearCalculator> updateStrategy;
        private IConvertStrategy<BeamShearCalculatorInputDataDTO, IBeamShearCalculatorInputData> inputDataConvertStrategy;

        public BeamShearCalculatorToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearCalculatorDTO GetNewItem(IBeamShearCalculator source)
        {
            try
            {
                GetNewCalculator(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void GetNewCalculator(IBeamShearCalculator source)
        {
            TraceLogger?.AddMessage($"Beam shear calculator converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData);
            TraceLogger?.AddMessage($"Beam shear calculator converting Id = {NewItem.Id} has been finished successfully", TraceLogStatuses.Debug);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            inputDataConvertStrategy = new DictionaryConvertStrategy<BeamShearCalculatorInputDataDTO, IBeamShearCalculatorInputData>
                (this, new BeamShearCalculatorInputDataToDTOConvertStrategy(this));
        }
    }
}
