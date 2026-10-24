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
    internal class ConstructionStageInputDataToDTOConvertStrategy : ConvertStrategy<ConstructionStageInputDataDTO, IConstructionStageInputData>
    {
        private IProcessLogic<IHasPrimitives> primitivesLogic;
        private IProcessLogic<IHasPrimitives> PrimitivesLogic => primitivesLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IConvertStrategy<IForceAction, IForceAction> forceConvertStrategy;

        public ConstructionStageInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public ConstructionStageDTO ConstructionStageDTO { get; set; }

        public override ConstructionStageInputDataDTO GetNewItem(IConstructionStageInputData source)
        {
            CheckObject.ThrowIfNull(source);
            CheckObject.ThrowIfNull(source.Primitives);
            CheckObject.ThrowIfNull(source.ForceAction);
            ChildClass = this;
            NewItem = new(source.Id, ConstructionStageDTO);
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

        private void ProcessPrimitives(IConstructionStageInputData source)
        {
            PrimitivesLogic.Source = source;
            PrimitivesLogic.Target = NewItem;
            PrimitivesLogic.Process();
        }
    }
}
