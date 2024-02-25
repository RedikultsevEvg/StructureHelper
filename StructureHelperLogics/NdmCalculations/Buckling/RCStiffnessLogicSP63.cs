using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class RCStiffnessLogicSP63 : IRCStiffnessLogic
    {
        const double initialKs = 0.7d;
        const double initialKc = 0.15d;
        const double deltaEAddition = 0.3d;

        IConcretePhiLLogic phiLLogic { get; }
        IConcreteDeltaELogic deltaELogic { get; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public RCStiffnessLogicSP63() : this(new ConstPhiLLogic(), new ConstDeltaELogic()) { }

        public RCStiffnessLogicSP63(IConcretePhiLLogic phiLLogic, IConcreteDeltaELogic deltaELogic)
        {
            this.phiLLogic = phiLLogic;
            this.deltaELogic = deltaELogic;
        }

        public (double Kc, double Ks) GetStiffnessCoeffitients()
        {
            if (TraceLogger is not null)
            {
                phiLLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                deltaELogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            double phiL = phiLLogic.GetPhil();
            double deltaE = deltaELogic.GetDeltaE();
            TraceLogger?.AddMessage(string.Format("Factor of relative eccentricity DeltaE = {0}", deltaE));
            double kc = initialKc / (phiL * (deltaEAddition + deltaE));
            var messageString = string.Format("Factor of stiffness of concrete Kc = {0} / ({1} * ({2} + {3}))  = {4}, {5}", initialKc, phiL, deltaEAddition, deltaE, kc, LoggerStrings.DimensionLess);
            TraceLogger?.AddMessage(messageString);
            return (kc, initialKs);
        }
    }
}
