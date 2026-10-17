using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorToDTOConvertStrategy : ConvertStrategy<HistoryCalculatorDTO, IHistoryCalculator>
    {
        private IUpdateStrategy<IHistoryCalculator> updateStrategy;
        private IUpdateStrategy<IHistoryCalculator> UpdateStrategy=> updateStrategy ??= new HistoryCalculatorUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ConstructionStageDTO, IConstructionStage> stageConvertStrategy;
        private IConvertStrategy<ConstructionStageDTO, IConstructionStage> StageConvertStrategy => stageConvertStrategy ??= new ConstructionStageToDTOConvertStrategy(this);
        public override HistoryCalculatorDTO GetNewItem(IHistoryCalculator source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            CheckObject.ThrowIfNull(source.ConstructionStages);
            CheckObject.ThrowIfNull(NewItem.ConstructionStages);
            foreach (var item in source.ConstructionStages)
            {
                var newStage = StageConvertStrategy.Convert(item);
                NewItem.ConstructionStages.Add(newStage);
            }
            return NewItem;
        }
    }
}
