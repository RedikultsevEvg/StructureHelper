using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupByDensityCheckLogic : ICheckEntityLogic<IStirrupByDensity>
    {
        private const int minDensity = 0;
        private bool result;
        private string checkResult;

        public StirrupByDensityCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IStirrupByDensity Entity { get; set; }

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
                if (Entity.StirrupDensity < minDensity)
                {
                    result = false;
                    TraceMessage($"\nStirrup {Entity.Name} density d = {Entity.StirrupDensity} must not be less than dmin = {minDensity}");
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
