using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class CheckAccuracyLogic : ICheckEntityLogic<IAccuracy>
    {
        private string checkResult;
        private bool result;

        public IAccuracy Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckAccuracyLogic(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public CheckAccuracyLogic() : this (null)
        {
            
        }

        public bool Check()
        {
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nAccuracy requirements is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.IterationAccuracy <= 0)
                {
                    result = false;
                    TraceMessage($"\nValue of accuracy {Entity.IterationAccuracy} must be grater than zero");
                }
                if (Entity.MaxIterationCount <= 0)
                {
                    result = false;
                    TraceMessage($"\nMax number of iteration {Entity.MaxIterationCount} must be grater than zero");
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
