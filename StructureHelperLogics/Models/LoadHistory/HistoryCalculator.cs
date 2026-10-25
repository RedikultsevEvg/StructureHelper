using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

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

        public HistoryCalculator(Guid id, IShiftTraceLogger traceLogger = null)
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

        public object Clone()
        {
            HistoryCalculatorUpdateStrategy updateStrategy = new() { UpdateChildren = true };
            HistoryCalculator newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

    }
}
