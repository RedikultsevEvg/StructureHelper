using DataAccess.DTOs.Converters;
using NLog.Targets;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class ConstructionStageToDTOConvertStrategy : ConvertStrategy<ConstructionStageDTO, IConstructionStage>
    {
        private IUpdateStrategy<IConstructionStage> updateStrategy;
        private IUpdateStrategy<IConstructionStage> UpdateStrategy => updateStrategy ??= new ConstructionStageUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ConstructionStageInputDataDTO, IConstructionStageInputData> inputDataConvertStrategy;
        private IConvertStrategy<ConstructionStageInputDataDTO, IConstructionStageInputData> InputDataConvertStrategy => inputDataConvertStrategy ??= new ConstructionStageInputDataToDTOConvertStrategy(this) { ConstructionStageDTO = NewItem };

        public ConstructionStageToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConstructionStageDTO GetNewItem(IConstructionStage source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.InputData);
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.InputData = InputDataConvertStrategy.Convert(source.InputData);
            return NewItem;
        }
    }
}
