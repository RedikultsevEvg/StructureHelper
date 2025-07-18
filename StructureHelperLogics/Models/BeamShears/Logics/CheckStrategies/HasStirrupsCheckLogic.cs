using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasStirrupsCheckLogic : ICheckEntityLogic<IHasStirrups>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IStirrup> checkStirrupLogic;

        public IHasStirrups Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public HasStirrupsCheckLogic(IShiftTraceLogger? traceLogger)
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
                string errorString = "\nHasSturrup element is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.Stirrups is null)
                {
                    result = false;
                    TraceMessage("\nCollection of stirrups is null");
                }
                checkStirrupLogic ??= new StirrupsCheckLogic(TraceLogger);
                foreach (var stirrup in Entity.Stirrups)
                {
                    checkStirrupLogic.Entity = stirrup;
                    if (checkStirrupLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkStirrupLogic.CheckResult;
                    }
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
