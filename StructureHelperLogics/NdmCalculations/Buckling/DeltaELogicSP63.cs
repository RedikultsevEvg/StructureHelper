using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class DeltaELogicSP63 : IConcreteDeltaELogic
    {
        const double deltaEMin = 0.15d;
        const double deltaEMax = 1.5d;

        readonly double eccentricity;
        readonly double size;
        public IShiftTraceLogger? TraceLogger { get; set; }
        public DeltaELogicSP63(double eccentricity, double size)
        {
            if (size <= 0 )
            {
                TraceLogger?.AddMessage(string.Format("Height of cross-section is less or equal to zero, h = {0}", size));
                throw new StructureHelperException(ErrorStrings.SizeMustBeGreaterThanZero + $", actual size: {size}");
            }
            this.eccentricity = eccentricity;
            this.size = size;
        }


        public double GetDeltaE()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(string.Format("Eccentricity e = {0}", eccentricity));
            TraceLogger?.AddMessage(string.Format("Height h = {0}", size));
            var deltaE = Math.Abs(eccentricity) / size;
            string message = string.Format("Relative eccentricity DeltaE = eccentricity / height = {0} / {1} = {2} (dimensionless)", eccentricity, size, deltaE);
            TraceLogger?.AddMessage(message);
            deltaE = Math.Max(deltaE, deltaEMin);
            deltaE = Math.Min(deltaE, deltaEMax);
            TraceLogger?.AddMessage(string.Format("But not less than {0}, and not greater than {1}", deltaEMin, deltaEMax));
            TraceLogger?.AddMessage(string.Format("Relative eccentricity DeltaE = {0}", deltaE));
            return deltaE;
        }
    }
}
