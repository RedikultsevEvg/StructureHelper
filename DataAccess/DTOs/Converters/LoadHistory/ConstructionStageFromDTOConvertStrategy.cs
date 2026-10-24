using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    internal class ConstructionStageFromDTOConvertStrategy : ConvertStrategy<ConstructionStage, ConstructionStageDTO>
    {
        private IUpdateStrategy<IConstructionStage> updateStrategy;
        private IUpdateStrategy<IConstructionStage> UpdateStrategy => updateStrategy ??= new ConstructionStageUpdateStrategy() { UpdateChildren = false };

        private IConvertStrategy<ConstructionStageInputData, ConstructionStageInputDataDTO> inputDataConvertStrategy;
        private IConvertStrategy<ConstructionStageInputData, ConstructionStageInputDataDTO> InputDataConvertStrategy => inputDataConvertStrategy ??= new ConstructionStageInputDataFromDTOConvertStrategy(this) { ConstructionStage = NewItem};

        public ConstructionStageFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConstructionStage GetNewItem(ConstructionStageDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.InputData is not ConstructionStageInputDataDTO inputDataDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.InputData));
            }
            NewItem.InputData = InputDataConvertStrategy.Convert(inputDataDTO);
            return NewItem;
        }
    }
}
