using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class CheckBeamShearActionLogic : ICheckEntityLogic<IBeamShearAction>
    {
        private string checkResult;
        private bool result;

        public string CheckResult => checkResult;
        public IBeamShearAction Entity { get; set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckBeamShearActionLogic(IShiftTraceLogger? traceLogger)
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
                string errorString = "\nInclined section is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.ExternalForce is null)
                {
                    result = false;
                    TraceMessage($"\nExternal force is null");
                }
                if (Entity.SupportAction is null)
                {
                    result = false;
                    TraceMessage($"\nSupport action is null");
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
