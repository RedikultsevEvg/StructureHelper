using DataAccess.DTOs.Converters;
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
    internal class ConstructionStageInputDataFromDTOConvertStrategy : ConvertStrategy<ConstructionStageInputData, ConstructionStageInputDataDTO>
    {
        public IConstructionStage ConstructionStage { get; set; }
        private IUpdateStrategy<IConstructionStageInputData> updateStrategy;
        private IUpdateStrategy<IConstructionStageInputData> UpdateStrategy => updateStrategy ??= new ConstructionStageInputDataUpdateStrategy() { UpdateChildren = false };
        private IProcessLogic<IHasPrimitives> primitivesLogic;
        private IProcessLogic<IHasPrimitives> PrimitivesLogic => primitivesLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IConvertStrategy<IForceAction, IForceAction> forceConvertStrategy;

        public ConstructionStageInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConstructionStageInputData GetNewItem(ConstructionStageInputDataDTO source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.Primitives);
            CheckObject.ThrowIfNull(source.ForceAction);
            NewItem = new(source.Id, ConstructionStage);
            UpdateStrategy.Update(NewItem, source);
            ProcessPrimitives(source);
            if (source.ForceAction is not null)
            {
                forceConvertStrategy = new ForceActionFromDTOConvertStrategy()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
                NewItem.ForceAction = forceConvertStrategy.Convert(source.ForceAction);
            }
            return NewItem;
        }

        private void ProcessPrimitives(IConstructionStageInputData source)
        {
            PrimitivesLogic.Source = source;
            PrimitivesLogic.Target = NewItem;
            PrimitivesLogic.Process();
        }
    }
}
