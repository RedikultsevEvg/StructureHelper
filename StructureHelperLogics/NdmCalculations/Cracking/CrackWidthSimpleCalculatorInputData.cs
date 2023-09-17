using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthSimpleCalculatorInputData : ICrackWidthSimpleCalculatorInputData
    {
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public StrainTuple StrainTuple { get; set; }
        public double PsiSFactor { get; set; }
        public double Length { get; set; }
        public SectionStressStates StressState { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
    }
}
