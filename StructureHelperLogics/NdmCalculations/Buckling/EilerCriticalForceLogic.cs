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
    internal class EilerCriticalForceLogic : IEilerCriticalForceLogic
    {
        public double LongitudinalForce { get; set; }
        public double StiffnessEI { get; set; }
        public double DesignLength { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetCriticalForce()
        {
            double Ncr = - Math.Pow(Math.PI, 2) * StiffnessEI / Math.Pow(DesignLength, 2);
            string message = string.Format("Ncr = - (PI ^ 2) * D / L0 ^2 = - ({0}^2 * {1} / ({2} ^2)) = {3}, N", Math.PI, StiffnessEI, DesignLength, Ncr);
            TraceLogger?.AddMessage(message);
            return Ncr;
        }

        public double GetEtaFactor()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (LongitudinalForce >= 0d) return 1d;
            var Ncr = GetCriticalForce();
            if (LongitudinalForce <= Ncr)
            {
                string error = string.Format("Absolute value of longitudinalForce is greater or equal than critical force N = {0} => Ncr = {1}", Math.Abs(LongitudinalForce), Ncr);
                TraceLogger?.AddMessage(error, TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.LongitudinalForceMustBeLessThanCriticalForce);
            }
            double eta = 1 / (1 - LongitudinalForce / Ncr);
            string message = string.Format("Eta factor Eta = 1 / (1 - N / Ncr) = 1 / (1 - {0} / {1}) = {2}, {3}", (-1) * LongitudinalForce, (-1) * Ncr, eta, LoggerStrings.DimensionLess);
            TraceLogger?.AddMessage(message);
            return eta;
        }
    }
}
