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
        private IProcessLogic<IHasPrimitives> primitivesLogic;
        private IProcessLogic<IHasPrimitives> PrimitivesLogic => primitivesLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IConvertStrategy<IForceAction, IForceAction> forceConvertStrategy;

        public ConstructionStageToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConstructionStageDTO GetNewItem(IConstructionStage source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            ProcessPrimitives(source);
            if (source.ForceAction is not null)
            {
                forceConvertStrategy = new ForceActionToDTOConvertStrategy()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
                NewItem.ForceAction = forceConvertStrategy.Convert(source.ForceAction);
            }
            return NewItem;
        }

        private void ProcessPrimitives(IConstructionStage source)
        {          
            PrimitivesLogic.Source = source;
            PrimitivesLogic.Target = NewItem;
            PrimitivesLogic.Process();
        }
    }
}
