using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class ConstDeltaELogic : IConcreteDeltaELogic
    {
        private const double deltaE = 1.5d;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetDeltaE()
        {
            var message = string.Format("Simple method of calculatinf of effect of eccentricity, DeltaE = {0}, dimensionless", deltaE);
            TraceLogger?.AddMessage(message);
            return deltaE;
        }
    }
}
