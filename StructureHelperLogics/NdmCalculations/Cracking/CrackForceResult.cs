using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackForceResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public bool IsSectionCracked { get; set; }
        public double FactorOfCrackAppearance { get; set; }
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IForceTuple TupleOfCrackAppearance { get; set; }
        public StrainTuple CrackedStrainTuple { get; set; }
        public StrainTuple ReducedStrainTuple { get; set; }
        public StrainTuple SofteningFactors { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public double PsiS { get; set; }
        
    }
}
