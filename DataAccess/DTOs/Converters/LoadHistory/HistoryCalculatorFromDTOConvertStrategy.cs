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
        private IConvertStrategy<ConstructionStage, ConstructionStageDTO> stageConvertStrategy;
        private IConvertStrategy<ConstructionStage, ConstructionStageDTO> StageConvertStrategy => stageConvertStrategy ??= new ConstructionStageFromDTOConvertStrategy(this);
        public override HistoryCalculator GetNewItem(HistoryCalculatorDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            CheckObject.ThrowIfNull(source.ConstructionStages);
            CheckObject.ThrowIfNull(NewItem.ConstructionStages);
            foreach (var item in source.ConstructionStages)
            {
                if (item is not ConstructionStageDTO constructionStage)
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(item) + $": construction stage, id {item.Id}, name = {item.Name}");
                }
                var newStage = StageConvertStrategy.Convert(constructionStage);
                NewItem.ConstructionStages.Add(newStage);
            }
            return NewItem;
        }
    }
}
