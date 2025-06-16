using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculator : IBeamShearCalculator
    {
        private IUpdateStrategy<IBeamShearCalculator> updateStrategy;
        private ICheckInputDataLogic<IBeamShearCalculatorInputData> checkInputDataLogic;
        private IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult> calculationLogic;
        private IBeamShearCalculatorResult result;

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public IBeamShearCalculatorInputData InputData { get; set; } = new BeamShearCalculatorInputData(Guid.NewGuid());

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public bool ShowTraceData { get; set; } = false;

        public BeamShearCalculator(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            BeamShearCalculator newItem = new(Guid.NewGuid());
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            PrepareNewResult();
            InitializeStrategies();
            if (CheckInputData() == false) { return;}
            try
            {
                CalculateResult();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
            }
        }

        private bool CheckInputData()
        {
            var checkResult = checkInputDataLogic.Check();
            if (checkResult == false)
            {
                result.IsValid = false;
                result.Description += checkInputDataLogic.CheckResult;
                TraceLogger?.AddMessage(checkInputDataLogic.CheckResult, TraceLogStatuses.Error);
            }
            return checkResult;
        }

        private void CalculateResult()
        {
            result = calculationLogic.GetResultByInputData(InputData);
        }

        private void InitializeStrategies()
        {
            checkInputDataLogic ??= new CheckBeamShearCalculatorInputDataLogic(InputData, TraceLogger);
            calculationLogic ??= new BeamShearCalculatorLogic(TraceLogger);
        }

        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty,
                InputData = InputData
            };
        }
    }
}
