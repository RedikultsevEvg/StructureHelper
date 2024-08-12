using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackCalculator : IRebarCrackCalculator
    {
        private ICrackWidthCalculationLogic crackWidthCalculationLogic;
        private RebarCrackResult result;
        private ICheckInputDataLogic<IRebarCrackCalculatorInputData> checkInputDataLogic;

        public string Name { get; set; }
        public RebarCrackCalculatorInputData InputData { get; set; }
        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public RebarCrackCalculator(ICheckInputDataLogic<IRebarCrackCalculatorInputData> checkInputDataLogic,
            ICrackWidthCalculationLogic crackWidthCalculationLogic,
            IShiftTraceLogger? traceLogger)
        {
            this.checkInputDataLogic = checkInputDataLogic;
            this.crackWidthCalculationLogic = crackWidthCalculationLogic;
            this.TraceLogger = traceLogger;
        }

        public RebarCrackCalculator() 
            : this(new CheckRebarCrackCalculatorInputDataLogic(),
                  new CrackWidthCalculationLogic(),
                  null)
        {
            
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage($"Rebar primitive {InputData.RebarPrimitive.Name}");
            PrepareNewResult();
           if (CheckInputData() != true)
            {
                return;
            }

            try
            {
                ProcessCrackWidthCalculation();
            }
            catch (Exception ex)
            {
                ProcessIncorrectCalculation(ex);
            }
            result.RebarPrimitive = InputData.RebarPrimitive;
        }

        private bool CheckInputData()
        {
            checkInputDataLogic.InputData = InputData;
            checkInputDataLogic.TraceLogger = TraceLogger;
            if (checkInputDataLogic.Check() == true)
            {
                return true;
            }
            result.IsValid = false;
            result.Description += checkInputDataLogic.CheckResult;
            return false;
        }

        private void ProcessIncorrectCalculation(Exception ex)
        {
            TraceLogger?.AddMessage($"Error of crack width calculation {ex}", TraceLogStatuses.Error);
            result.IsValid = false;
            result.Description += "\n" + ex;
        }

        private void ProcessCrackWidthCalculation()
        {
            crackWidthCalculationLogic.TraceLogger = TraceLogger;
            crackWidthCalculationLogic.InputData = InputData;
            crackWidthCalculationLogic.Run();
            if (crackWidthCalculationLogic.Result.IsValid == true)
            {
                result = crackWidthCalculationLogic.Result;
                return;
            }
            result.IsValid = false;
            result.Description += crackWidthCalculationLogic.Result.Description;
        }

        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true,
                Description = string.Empty,
            };
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
