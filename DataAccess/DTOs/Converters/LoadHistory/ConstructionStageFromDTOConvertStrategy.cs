using DataAccess.DTOs.Converters;
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
        private IProcessLogic<IHasPrimitives> primitivesLogic;
        private IProcessLogic<IHasPrimitives> PrimitivesLogic => primitivesLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IConvertStrategy<IForceAction, IForceAction> forceConvertStrategy;

        public ConstructionStageFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ConstructionStage GetNewItem(ConstructionStageDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
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

        private void ProcessPrimitives(IConstructionStage source)
        {
            PrimitivesLogic.Source = source;
            PrimitivesLogic.Target = NewItem;
            PrimitivesLogic.Process();
        }
    }
}
