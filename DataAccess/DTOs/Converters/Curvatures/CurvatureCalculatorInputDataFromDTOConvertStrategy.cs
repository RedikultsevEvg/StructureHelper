using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<CurvatureCalculatorInputData, CurvatureCalculatorInputDataDTO>
    {
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IUpdateStrategy<ICurvatureCalculatorInputData> updateStrategy;
        private IConvertStrategy<DeflectionFactor, DeflectionFactorDTO> deflectionConvertStrategy;

        public CurvatureCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IHasPrimitivesProcessLogic PrimitivesProcessLogic => primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IHasForceActionsProcessLogic ActionsProcessLogic => actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IUpdateStrategy<ICurvatureCalculatorInputData> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorInputDataUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<DeflectionFactor, DeflectionFactorDTO> DeflectionConvertStrategy => deflectionConvertStrategy ??= new DeflectionFactorFromDTOConvertStrategy(this);
        public override CurvatureCalculatorInputData GetNewItem(CurvatureCalculatorInputDataDTO source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.DeflectionFactor is not DeflectionFactorDTO deflectionFactorDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.DeflectionFactor) + ": deflection factor");
            }
            NewItem.DeflectionFactor = DeflectionConvertStrategy.Convert(deflectionFactorDTO);
            ProcessPrimitives(source);
            ProcessActions(source);
            return NewItem;
        }

        private void ProcessPrimitives(IHasPrimitives source)
        {
            PrimitivesProcessLogic.Source = source;
            PrimitivesProcessLogic.Target = NewItem;
            PrimitivesProcessLogic.Process();
        }
        private void ProcessActions(IHasForceActions source)
        {
            ActionsProcessLogic.Source = source;
            ActionsProcessLogic.Target = NewItem;
            ActionsProcessLogic.Process();
        }
    }
}
