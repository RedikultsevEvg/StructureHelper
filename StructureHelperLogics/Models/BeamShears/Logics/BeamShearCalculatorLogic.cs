using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorLogic : IGetResultByInputDataLogic<IBeamShearCalculatorInputData, IBeamShearCalculatorResult>
    {
        private IBeamShearCalculatorResult result;
        private List<IBeamShearActionResult> actionResults;
        private readonly IActionResultListLogic getActionResultListLogic;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public BeamShearCalculatorLogic(IActionResultListLogic getActionResultListLogic, IShiftTraceLogger? traceLogger)
        {
            this.getActionResultListLogic = getActionResultListLogic;
            TraceLogger = traceLogger;
        }


        public IBeamShearCalculatorResult GetResultByInputData(IBeamShearCalculatorInputData inputData)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);

            PrepareNewResult();
            try
            {
                GetActionResults();
                result.ActionResults = actionResults;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.IsValid = false;
                result.Description += "\n" + ex.Message;
            }

            return result;
        }

        private void GetActionResults()
        {
            actionResults = getActionResultListLogic.GetActionResults();
        }


        private void PrepareNewResult()
        {
            result = new BeamShearCalculatorResult()
            {
                IsValid = true,
                Description = string.Empty
            };
        }
    }
}
