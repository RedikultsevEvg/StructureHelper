using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthLogicSP63 : ICrackWidthLogic
    {
        public double RebarStrain { get; set; }
        public double ConcreteStrain { get; set; }
        public double Length { get; set; }
        public double TermFactor { get; set; }
        public double BondFactor { get; set; }
        public double StressStateFactor { get; set; }
        public double PsiSFactor { get; set; }
        public double GetCrackWidth()
        {
            CheckOptions();
            //check if strain of concrete greater than strain of rebar
            if (ConcreteStrain > RebarStrain) { return 0d; }
            double width = (RebarStrain - ConcreteStrain) * Length;
            width *= TermFactor * BondFactor * StressStateFactor * PsiSFactor;
            return width;
        }

        private void CheckOptions()
        {
            if (Length <=0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": length between cracks L={Length} must be greate than zero");
            }
            if (TermFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Term factor {TermFactor} must be greate than zero");
            }
            if (BondFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Bond factor {BondFactor} must be greate than zero");
            }
            if (StressStateFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Bond factor {StressStateFactor} must be greate than zero");
            }
            if (PsiSFactor <= 0d)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": PsiS factor {PsiSFactor} must be greate than zero");
            }
        }
    }
}
