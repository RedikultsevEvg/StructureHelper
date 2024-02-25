using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class ConstPhiLLogic : IConcretePhiLLogic
    {
        private const double phiL = 2.0d;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetPhil()
        {
            var message = string.Format("Simple method of calculatinf of effect of long term load, PhiL = {0}, dimensionless", phiL);
            TraceLogger?.AddMessage(message);
            return phiL;
        }
    }
}
