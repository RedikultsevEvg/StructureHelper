using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<HistoryCalculatorInputData, HistoryCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IHistoryCalculatorInputData> updateStrategy;
        private IUpdateStrategy<IHistoryCalculatorInputData> UpdateStrategy => updateStrategy ??= new HistoryCalculatorInputDataUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ConstructionStage, ConstructionStageDTO> stageConvertStrategy;

        public HistoryCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IConvertStrategy<ConstructionStage, ConstructionStageDTO> StageConvertStrategy => stageConvertStrategy ??= new ConstructionStageFromDTOConvertStrategy(this);
        public IHistoryCalculator HistoryCalculator { get; set; }

        public override HistoryCalculatorInputData GetNewItem(HistoryCalculatorInputDataDTO source)
        {
            CheckObject.ThrowIfNull(HistoryCalculator);
            NewItem = new(source.Id, HistoryCalculator);
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
