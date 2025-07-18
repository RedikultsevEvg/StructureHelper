using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroupCheckLogic : ICheckEntityLogic<IStirrupGroup>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IHasStirrups> hasStirrupsCheckLogic;
        public IStirrupGroup Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupGroupCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            checkResult = string.Empty;
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nStirrup group is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                hasStirrupsCheckLogic ??= new HasStirrupsCheckLogic(TraceLogger);
                hasStirrupsCheckLogic.Entity = Entity;
                if (hasStirrupsCheckLogic.Check() == false)
                {
                    result = false;
                    checkResult += "\nStirrup group has some errors";
                    checkResult += hasStirrupsCheckLogic.CheckResult;
                }
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
