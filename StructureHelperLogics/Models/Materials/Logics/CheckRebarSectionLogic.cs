using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal class CheckRebarSectionLogic : ICheckEntityLogic<IRebarSection>
    {
        private bool result;
        private string checkResult;
        public IRebarSection Entity { get; set; }

        public string CheckResult => checkResult;
        public double MinDiameter { get; set; } = 0.003;
        public double MaxDiameter { get; set; } = 0.090;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckRebarSectionLogic(IShiftTraceLogger? traceLogger)
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
                string errorString = "\nRebar section is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.Diameter < MinDiameter)
                {
                    result = false;
                    TraceMessage($"\nRebar diameter d = {Entity.Diameter} must not be less than dmin = {MinDiameter}");
                }
                if (Entity.Diameter > MaxDiameter)
                {
                    result = false;
                    TraceMessage($"\nRebar diameter d = {Entity.Diameter} must be less or equal than dmax = {MaxDiameter}");
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
