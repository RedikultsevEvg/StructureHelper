using StructureHelperCommon.Infrastructures.Exceptions;
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
        public DeltaELogicSP63(double eccentricity, double size)
        {
            if (size <= 0 )
            {
                throw new StructureHelperException(ErrorStrings.SizeMustBeGreaterThanZero + $", actual size: {size}");
            }
            this.eccentricity = eccentricity;
            this.size = size;
        }

        public double GetDeltaE()
        {
            var deltaE = Math.Abs(eccentricity) / size;
            deltaE = Math.Max(deltaE, deltaEMin);
            deltaE = Math.Min(deltaE, deltaEMax);
            return deltaE;
        }
    }
}
