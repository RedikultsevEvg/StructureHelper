using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorToDTOConvertStrategy : ConvertStrategy<HistoryCalculatorDTO, IHistoryCalculator>
    {
        private IUpdateStrategy<IHistoryCalculator> updateStrategy;
        private IUpdateStrategy<IHistoryCalculator> UpdateStrategy=> updateStrategy ??= new HistoryCalculatorUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<HistoryCalculatorInputDataDTO, IHistoryCalculatorInputData> inputDataConvertStrategy;
        private IConvertStrategy<HistoryCalculatorInputDataDTO, IHistoryCalculatorInputData> InputDataConvertStrategy => inputDataConvertStrategy ??= new HistoryCalculatorInputDataToDTOConvertStrategy(this) { HistoryCalculatorDTO = NewItem};
        public override HistoryCalculatorDTO GetNewItem(IHistoryCalculator source)
        {
            ChildClass = this;
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.InputData);
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.InputData = InputDataConvertStrategy.Convert(source.InputData);
            return NewItem;
        }
    }
}
