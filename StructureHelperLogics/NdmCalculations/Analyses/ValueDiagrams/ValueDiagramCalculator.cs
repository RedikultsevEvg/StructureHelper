using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculator : IValueDiagramCalculator
    {
        private readonly IValueDiagramCalculatorLogic valueDiagramCalculatorLogic = new ValueDiagramCalculatorLogic();
        private readonly ICheckInputDataLogic<IValueDiagramCalculatorInputData> checkInputDataLogic;
        private IValueDiagramCalculatorResult result;

        public Guid Id { get; }
        public string Name { get; set; }
        public bool ShowTraceData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramCalculatorInputData InputData { get; set; } = new ValueDiagramCalculatorInputData(Guid.NewGuid());

        public ValueDiagramCalculator(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            ValueDiagramCalculator newItem = new ValueDiagramCalculator(Guid.NewGuid());
            var updateLogic = new ValueDiagramCalculatorUpdateStrategy();
            updateLogic.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            valueDiagramCalculatorLogic.InputData = InputData;
            result = valueDiagramCalculatorLogic.GetResult();
        }
    }
}
