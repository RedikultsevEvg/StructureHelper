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
                CheckEntity();
            }

            return result;
        }

        private void CheckEntity()
        {
            if (Entity.StirrupDensity < minDensity)
            {
                result = false;
                TraceMessage($"\nStirrup {Entity.Name} density d = {Entity.StirrupDensity} must not be less than dmin = {minDensity}");
            }
            if (Entity.StartCoordinate < 0)
            {
                result = false;
                TraceMessage($"\nStirrup {Entity.Name} start coordinate must not be less than zero, but was {Entity.StartCoordinate}");
            }
            if (Entity.EndCoordinate <= Entity.StartCoordinate)
            {
                result = false;
                TraceMessage($"\nStirrup {Entity.Name} start coordinate must be less than end coordinte, but was Xstart = {Entity.StartCoordinate}(m), Xend = {Entity.EndCoordinate}(m)");
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
