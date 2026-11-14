using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculator : IValueDiagramCalculator
    {
        private readonly IValueDiagramCalculatorLogic valueDiagramCalculatorLogic = new ValueDiagramCalculatorLogic();
        private readonly ICheckEntityLogic<IValueDiagramCalculatorInputData> checkInputDataLogic;


        private IValueDiagramCalculatorResult result;

        public Guid Id { get; }
        public string Name { get; set; }
        public bool ShowTraceData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public IValueDiagramCalculatorInputData InputData { get; set; } = new ValueDiagramCalculatorInputData(Guid.NewGuid());

        public ValueDiagramCalculator(ICheckEntityLogic<IValueDiagramCalculatorInputData> checkInputDataLogic)
        {
            this.checkInputDataLogic = checkInputDataLogic;
        }

        public ValueDiagramCalculator(Guid id)
        {
            Id = id;
            checkInputDataLogic = new ValueDiagramInputDataCheckLogic();
        }

        public void Run()
        {
            checkInputDataLogic.Entity = InputData;
            if (checkInputDataLogic.Check() != true)
            {
                result = new ValueDiagramCalculatorResult()
                {
                    IsValid = false,
                    Description = checkInputDataLogic.CheckResult
                };
                return;
            }
            valueDiagramCalculatorLogic.InputData = InputData;
            result = valueDiagramCalculatorLogic.GetResult();
        }

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ValueDiagramCalculator newItem = new(id);
            var updateLogic = new ValueDiagramCalculatorUpdateStrategy();
            updateLogic.Update(newItem, this);
            return newItem;
        }
    }
}
