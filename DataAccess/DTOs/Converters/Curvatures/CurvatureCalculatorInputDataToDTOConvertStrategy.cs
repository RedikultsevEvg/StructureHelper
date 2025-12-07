using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<CurvatureCalculatorInputDataDTO, ICurvatureCalculatorInputData>
    {
        private IProcessLogic<IHasForcesAndPrimitives> actionsProcessLogic;
        private IUpdateStrategy<ICurvatureCalculatorInputData> updateStrategy;
        private IConvertStrategy<DeflectionFactorDTO, IDeflectionFactor> deflectionConvertStrategy; 

        private IProcessLogic<IHasForcesAndPrimitives> ForceAndPrimitivesLogic => actionsProcessLogic ??= new HasForcesAndPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IUpdateStrategy<ICurvatureCalculatorInputData> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorInputDataUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<DeflectionFactorDTO, IDeflectionFactor> DeflectionConvertStrategy => deflectionConvertStrategy ??= new DeflectionFactorToDTOConvertStrategy(this); 

        public CurvatureCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override CurvatureCalculatorInputDataDTO GetNewItem(ICurvatureCalculatorInputData source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.DeflectionFactor = DeflectionConvertStrategy.Convert(source.DeflectionFactor);
            ProcessActions(source);
            return NewItem;
        }

        private void ProcessActions(IHasForcesAndPrimitives source)
        {
            ForceAndPrimitivesLogic.Source = source;
            ForceAndPrimitivesLogic.Target = NewItem;
            ForceAndPrimitivesLogic.Process();
        }
    }
}
