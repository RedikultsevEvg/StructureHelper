using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace DataAccess.DTOs
{
    public class CrackCalculatorFromDTOConvertStrategy : ConvertStrategy<CrackCalculator, CrackCalculatorDTO>
    {
        private IConvertStrategy<CrackCalculatorInputData, CrackCalculatorInputDataDTO> convertStrategy;
        private IConvertStrategy<CrackCalculatorInputData, CrackCalculatorInputDataDTO> ConvertStrategy => convertStrategy ??= new CrackCalculatorInputDataFromDTOConvertStrategy();

        public override CrackCalculator GetNewItem(CrackCalculatorDTO source)
        {
            NewItem = new(source.Id, new ShiftTraceLogger());
            NewItem.Name = source.Name;
            ConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            ConvertStrategy.TraceLogger = TraceLogger;
            NewItem.InputData = ConvertStrategy.Convert(source.InputData as CrackCalculatorInputDataDTO);
            return NewItem;
        }
    }
}
