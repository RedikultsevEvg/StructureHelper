using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class CheckForceActionsLogic : ICheckEntityLogic<IEnumerable<IForceAction>>
    {
        private bool result;
        private string checkResult;
        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckForceActionsLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public CheckForceActionsLogic()
        {
            
        }

        public IEnumerable<IForceAction> Entity { get; set; }

        public bool Check()
        {
            result = true;
            foreach (var item in Entity)
            {
                try
                {
                    item.GetCombinations();
                }
                catch (Exception ex)
                {
                    result = false;
                    string errorString = $"Error of getting of combination of forces from action {item.Name}: {ex.Message}";
                    TraceMessage(errorString);
                }
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
