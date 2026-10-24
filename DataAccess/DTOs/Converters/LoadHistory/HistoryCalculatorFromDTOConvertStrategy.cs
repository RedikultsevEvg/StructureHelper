using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorFromDTOConvertStrategy : ConvertStrategy<HistoryCalculator, HistoryCalculatorDTO>
    {
        private IUpdateStrategy<IHistoryCalculator> updateStrategy;
        private IUpdateStrategy<IHistoryCalculator> UpdateStrategy => updateStrategy ??= new HistoryCalculatorUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<HistoryCalculatorInputData, HistoryCalculatorInputDataDTO> inputDataConvertStrategy;
        private IConvertStrategy<HistoryCalculatorInputData, HistoryCalculatorInputDataDTO> InputDataConvertStrategy => inputDataConvertStrategy ??= new HistoryCalculatorInputDataFromDTOConvertStrategy(this) { HistoryCalculator = NewItem};

        public override HistoryCalculator GetNewItem(HistoryCalculatorDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            CheckObject.ThrowIfNull(source.InputData);
            if (source.InputData is not HistoryCalculatorInputDataDTO sourceInputData)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.InputData));
            }
            NewItem.InputData = InputDataConvertStrategy.Convert(sourceInputData);
            return NewItem;
        }
    }
}
