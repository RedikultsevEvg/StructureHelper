using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorFromDTOConvertStrategy : ConvertStrategy<CurvatureCalculator, CurvatureCalculatorDTO>
    {
        IUpdateStrategy<ICurvatureCalculator> updateStrategy;
        IConvertStrategy<CurvatureCalculatorInputData, CurvatureCalculatorInputDataDTO> inputDataConvertStrategy;
        IUpdateStrategy<ICurvatureCalculator> UpdateStrategy => updateStrategy ??= new CurvatureCalculatorUpdateStrategy() { UpdateChildren = false };
        IConvertStrategy<CurvatureCalculatorInputData, CurvatureCalculatorInputDataDTO> InputDataConvertStrategy => inputDataConvertStrategy ??= new CurvatureCalculatorInputDataFromDTOConvertStrategy(this);

        public override CurvatureCalculator GetNewItem(CurvatureCalculatorDTO source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.InputData is not CurvatureCalculatorInputDataDTO inputData)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.InputData) + ": deflection factor");
            }
            NewItem.InputData = InputDataConvertStrategy.Convert(inputData);
            return NewItem;
        }
    }
}
