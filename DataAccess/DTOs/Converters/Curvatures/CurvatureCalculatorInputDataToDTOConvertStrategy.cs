using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<CurvatureCalculatorInputDataDTO, ICurvatureCalculatorInputData>
    {
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IUpdateStrategy<ICurvatureCalculatorInputData> updateStrategy;
        private IConvertStrategy<DeflectionFactorDTO, IDeflectionFactor> deflectionConvertStrategy; 

        private IHasPrimitivesProcessLogic PrimitivesProcessLogic => primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        private IHasForceActionsProcessLogic ActionsProcessLogic => actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        private IUpdateStrategy<ICurvatureCalculatorInputData> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorInputDataUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<DeflectionFactorDTO, IDeflectionFactor> DeflectionConvertStrategy => deflectionConvertStrategy ??= new DeflectionFactorToDTOConvertStrategy(this); 

        public CurvatureCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override CurvatureCalculatorInputDataDTO GetNewItem(ICurvatureCalculatorInputData source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.DeflectionFactor = DeflectionConvertStrategy.Convert(source.DeflectionFactor);
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
