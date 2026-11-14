using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculator : ICurvatureCalculator
    {
        private CurvatureCalculatorResult result;
        private ICheckEntityLogic<ICurvatureCalculatorInputData> inputDataCheckLogic;
        private ICurvatureCalcualtorLogic calculatorLogic;
        private ICurvatureCalcualtorLogic CalculatorLogic => calculatorLogic ??= new CurvatureCalculatorLogic() { InputData = InputData, TraceLogger = TraceLogger};

        private ICheckEntityLogic<ICurvatureCalculatorInputData> InputDataCheckLogic => inputDataCheckLogic ??= new CurvatureCalculatorInputDataCheckLogic(TraceLogger) { Entity = InputData};

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public ICurvatureCalculatorInputData InputData { get; set; } = new CurvatureCalculatorInputData(Guid.NewGuid());
        public bool ShowTraceData { get; set; } = false;

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }


        public CurvatureCalculator(Guid id)
        {
            Id = id;
        }

        public void Run()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            PrepareNewResult();
            if (CheckInputData() == false)  {return;}
            GetResultByLogic();
        }

        private void GetResultByLogic()
        {
            try
            {
                CalculatorLogic.Run();
                result = CalculatorLogic.Result as CurvatureCalculatorResult;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }

        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true,
                InputData = InputData,
            };
        }

        private bool CheckInputData()
        {
            if (InputDataCheckLogic.Check() == false)
            {
                result.IsValid = false;
                result.Description += inputDataCheckLogic.CheckResult;
                return false;
            }
            return true;
        }

        public ILogicCalculator GetClone(ILogicCalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            CurvatureCalculator newItem = new(id);
            var updateStrategy = new CurvatureCalculatorUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
