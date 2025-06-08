using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace DataAccess.DTOs
{
    public class ForceCalculatorFromDTOConvertStrategy : ConvertStrategy<ForceCalculator, ForceCalculatorDTO>
    {

        private IConvertStrategy<ForceCalculatorInputData, ForceCalculatorInputDataDTO> inputDataConvertStrategy;
        public override ForceCalculator GetNewItem(ForceCalculatorDTO source)
        {
            InitializeStrategies();
            try
            {
                GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void InitializeStrategies()
        {
            inputDataConvertStrategy ??= new ForceCalculatorInputDataFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }

        private void GetNewItemBySource(ForceCalculatorDTO source)
        {
            NewItem = new(source.Id);
            NewItem.Name = source.Name;
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData as ForceCalculatorInputDataDTO);
        }
    }
}
