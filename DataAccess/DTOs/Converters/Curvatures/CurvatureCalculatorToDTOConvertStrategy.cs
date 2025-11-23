using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorToDTOConvertStrategy : ConvertStrategy<CurvatureCalculatorDTO, ICurvatureCalculator>
    {
        IUpdateStrategy<ICurvatureCalculator> updateStrategy;
        IConvertStrategy<CurvatureCalculatorInputDataDTO, ICurvatureCalculatorInputData> inputDataConvertStrategy;
        IUpdateStrategy<ICurvatureCalculator> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorUpdateStrategy() { UpdateChildren = false};
        IConvertStrategy<CurvatureCalculatorInputDataDTO, ICurvatureCalculatorInputData> InputDataConvertStrategy => inputDataConvertStrategy ??= new CurvatureCalculatorInputDataToDTOConvertStrategy(this);
        

        public override CurvatureCalculatorDTO GetNewItem(ICurvatureCalculator source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.InputData = InputDataConvertStrategy.Convert(source.InputData);
            return NewItem;
        }
    }
}
