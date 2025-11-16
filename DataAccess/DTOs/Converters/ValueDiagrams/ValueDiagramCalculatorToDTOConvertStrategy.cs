using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    internal class ValueDiagramCalculatorToDTOConvertStrategy : ConvertStrategy<ValueDiagramCalculatorDTO, IValueDiagramCalculator>
    {
        private IUpdateStrategy<IValueDiagramCalculator> updateStrategy;
        private IConvertStrategy<ValueDiagramCalculatorInputDataDTO, IValueDiagramCalculatorInputData> inputDataConvertStrategy;
        public override ValueDiagramCalculatorDTO GetNewItem(IValueDiagramCalculator source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramCalculatorUpdateStrategy()
            {
                UpdateChildren = false
            };
            inputDataConvertStrategy ??= new DictionaryConvertStrategy<ValueDiagramCalculatorInputDataDTO, IValueDiagramCalculatorInputData>(
                this, 
                new ValueDiagramCalculatorInputDataToDTOConvertStrategy(this));
        }
    }
}
