using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<HistoryCalculatorInputDataDTO, IHistoryCalculatorInputData>
    {
        private IUpdateStrategy<IHistoryCalculatorInputData> updateStrategy;
        private IUpdateStrategy<IHistoryCalculatorInputData> UpdateStrategy => updateStrategy ??= new HistoryCalculatorInputDataUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ConstructionStageDTO, IConstructionStage> stageConvertStrategy;

        public HistoryCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IConvertStrategy<ConstructionStageDTO, IConstructionStage> StageConvertStrategy => stageConvertStrategy ??= new ConstructionStageToDTOConvertStrategy(this);
        public HistoryCalculatorDTO HistoryCalculatorDTO { get; set; }
        public override HistoryCalculatorInputDataDTO GetNewItem(IHistoryCalculatorInputData source)
        {
            ChildClass = this;
            NewItem = new(source.Id, HistoryCalculatorDTO);
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
