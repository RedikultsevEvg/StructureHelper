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
    public class EquivalentDiameterLogic : IAverageDiameterLogic
    {
        public IEnumerable<RebarNdm> Rebars { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetAverageDiameter()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            Check();
            var rebarArea = Rebars
                .Sum(x => x.Area);
            TraceLogger?.AddMessage($"Summary rebar area As = {rebarArea}(m^2)");
            var rebarCount = Rebars.Count();
            TraceLogger?.AddMessage($"Rebar count n = {rebarCount}");
            var averageArea = rebarArea / rebarCount;
            TraceLogger?.AddMessage($"Equivalent rebar area As,eq = {averageArea}");
            var diameter = 2d * Math.Sqrt(averageArea / Math.PI);
            TraceLogger?.AddMessage($"Equivalent rebar diameter ds,eq = 2 * Sqrt( PI / As,eq) = 2 * Sqrt( PI / {averageArea}) = {diameter}(m)");
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
