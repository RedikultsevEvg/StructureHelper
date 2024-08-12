using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthLogicInputDataSP63 : ICrackWidthLogicInputData
    {
        public double RebarStrain { get; set; }
        public double ConcreteStrain { get; set; }
        public double LengthBetweenCracks { get; set; }
        public double TermFactor { get; set; }
        public double BondFactor { get; set; }
        public double StressStateFactor { get; set; }
        public double PsiSFactor { get; set; }
    }
}
