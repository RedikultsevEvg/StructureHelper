using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class AverageDiameterLogic : IAverageDiameterLogic
    {
        public IEnumerable<RebarNdm> Rebars { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetAverageDiameter()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            Check();
            var rebarArea = Rebars
                .Sum(x => x.Area);
            TraceLogger?.AddMessage($"Summary rebar area As = {rebarArea}");
            var rebarCount = Rebars.Count();
            TraceLogger?.AddMessage($"Rebar count n = {rebarCount}");
            var averageArea = rebarArea / rebarCount;
            TraceLogger?.AddMessage($"Average rebar area As = {averageArea}");
            var diameter = Math.Sqrt(averageArea / Math.PI);
            TraceLogger?.AddMessage($"Average rebar diameter ds = {diameter}");
            return diameter;
        }
        private void Check()
        {
            if (!Rebars.Any())
            {
                string errorString = ErrorStrings.DataIsInCorrect + $": rebars count must be greater then zero";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
