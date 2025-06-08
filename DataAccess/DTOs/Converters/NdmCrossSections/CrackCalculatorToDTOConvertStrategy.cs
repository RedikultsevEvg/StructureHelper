using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CrackCalculatorToDTOConvertStrategy : ConvertStrategy<CrackCalculatorDTO, ICrackCalculator>
    {
        private IUpdateStrategy<ICrackCalculator> updateStrategy;
        private IConvertStrategy<CrackCalculatorInputDataDTO, ICrackCalculatorInputData> inputDataConvertStrategy;

        public CrackCalculatorToDTOConvertStrategy(IUpdateStrategy<ICrackCalculator> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public CrackCalculatorToDTOConvertStrategy() {  }
        public override CrackCalculatorDTO GetNewItem(ICrackCalculator source)
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
            updateStrategy ??= new CrackCalculatorUpdateStrategy();
            inputDataConvertStrategy ??= new CrackCalculatorInputDataToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }

        private void GetNewItemBySource(ICrackCalculator source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData);
        }

    }
}
