using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ValueDiagramCalcualtorFromDTOConvertStrategy : ConvertStrategy<ValueDiagramCalculator, ValueDiagramCalculatorDTO>
    {
        private IUpdateStrategy<IValueDiagramCalculator> updateStrategy;
        private IConvertStrategy<ValueDiagramCalculatorInputData, ValueDiagramCalculatorInputDataDTO> inputDataConvertStrategy;

        public override ValueDiagramCalculator GetNewItem(ValueDiagramCalculatorDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData as ValueDiagramCalculatorInputDataDTO);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramCalculatorUpdateStrategy() { UpdateChildren = false };
            inputDataConvertStrategy ??= new ValueDiagramCalculatorInputDataFromDTOConvertStrategy(this);
        }
    }
}
