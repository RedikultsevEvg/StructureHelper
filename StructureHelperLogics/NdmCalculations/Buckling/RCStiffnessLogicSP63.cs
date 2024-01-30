using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class RCStiffnessLogicSP63 : IRCStiffnessLogic
    {
        IConcretePhiLLogic phiLLogic { get; }
        IConcreteDeltaELogic deltaELogic { get; }

        public RCStiffnessLogicSP63() : this(new ConstPhiLLogic(), new ConstDeltaELogic()) { }

        public RCStiffnessLogicSP63(IConcretePhiLLogic phiLLogic, IConcreteDeltaELogic deltaELogic)
        {
            this.phiLLogic = phiLLogic;
            this.deltaELogic = deltaELogic;
        }

        public (double Kc, double Ks) GetStiffnessCoeffitients()
        {
            const double initialKs = 0.7d;
            const double initialKc = 0.15d;
            const double deltaEAddition = 0.3d;
            double phiL = phiLLogic.GetPhil();
            double deltaE = deltaELogic.GetDeltaE();
            double kc = initialKc / (phiL * (deltaEAddition + deltaE));
            return (kc, initialKs);
        }
    }
}
