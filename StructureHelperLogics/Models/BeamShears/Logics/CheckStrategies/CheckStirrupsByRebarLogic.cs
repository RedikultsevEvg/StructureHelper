using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class CheckStirrupsByRebarLogic : ICheckEntityLogic<IStirrupByRebar>
    {
        private const double minDiameter = 0.003;
        private const double maxDiameter = 0.025;
        private const double minSpacing = 0.020;
        private const double maxSpacing = 0.500;
        private const int minLegCount = 0;
        private bool result;
        private string checkResult;

        public CheckStirrupsByRebarLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IStirrupByRebar Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            checkResult = string.Empty;
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nStirrups by density is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.Diameter < minDiameter)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} diameter d = {Entity.Diameter} must not be less than dmin = {minDiameter}");
                }
                if (Entity.Diameter > maxDiameter)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} diameter d = {Entity.Diameter} must be less or equal than dmax = {maxDiameter}");
                }
                if (Entity.Spacing < minSpacing)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} spacing s = {Entity.Spacing} must not be less than smin = {minSpacing}");
                }
                if (Entity.Spacing > maxSpacing)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} spacing s = {Entity.Spacing} must be less or equal than smax = {maxSpacing}");
                }
                if (Entity.LegCount < minLegCount)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} leg count n = {Entity.LegCount} must not be less than nmin = {minLegCount}");
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
