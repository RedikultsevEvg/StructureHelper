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
    public interface ICrackWidthSimpleCalculatorInputData
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        StrainTuple StrainTuple { get; set; }
        double PsiSFactor { get; set; }
        /// <summary>
        /// Length between cracks in meters
        /// </summary>
        double Length { get; set; }
        SectionStressStates StressState { get; set; }
        RebarPrimitive RebarPrimitive { get; set; }
    }
}
