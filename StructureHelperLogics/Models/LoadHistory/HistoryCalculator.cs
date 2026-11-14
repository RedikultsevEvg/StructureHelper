using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory.Logics;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculator : IHistoryCalculator
    {
        private ICheckEntityLogic<IHistoryCalculatorInputData> checkInputDataLogic;
        private ICheckEntityLogic<IHistoryCalculatorInputData> CheckInputDataLogic => checkInputDataLogic ??= new HistoryCalculatorInputDataCheckLogic(TraceLogger);
        private IHistoryCalculatorResult result;
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public bool ShowTraceData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IResult Result => result;

        public IHistoryCalculatorInputData InputData { get; set; }

        public HistoryCalculator(Guid id)
            : this(id, null)
        {
        }

        public HistoryCalculator(Guid id, IShiftTraceLogger traceLogger)
        {
            Id = id;
            TraceLogger = traceLogger;
            InputData = new HistoryCalculatorInputData(Guid.NewGuid(), this);
        }
        public void Run()
        {
            CheckInputDataLogic.Entity = InputData;
            if (CheckInputDataLogic.Check() == false)
            {
                PrepareNotValidResult();
                result.Description += "\n" + checkInputDataLogic.CheckResult;
                return;
            }
            HistoryCalculatorLogic logic = new();
            result = logic.CalculateResult(InputData);
        }

        private void PrepareNotValidResult()
        {
            result = new HistoryCalculatorResult(InputData)
            {
                IsValid = false
            };
        }

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            HistoryCalculator newItem = new(id);
            HistoryCalculatorUpdateStrategy updateStrategy = new();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
