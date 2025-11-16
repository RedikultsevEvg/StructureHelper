using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams.Logics;

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

        public object Clone()
        {
            ValueDiagramCalculator newItem = new ValueDiagramCalculator(Guid.NewGuid());
            var updateLogic = new ValueDiagramCalculatorUpdateStrategy();
            updateLogic.Update(newItem, this);
            return newItem;
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
    }
}
