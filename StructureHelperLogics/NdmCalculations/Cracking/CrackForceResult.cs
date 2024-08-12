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
    /// <summary>
    /// Result of crack calculation
    /// </summary>
    public class CrackForceResult : IResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string Description { get; set; }
        /// <summary>
        /// True when section is cracked
        /// </summary>
        public bool IsSectionCracked { get; set; }
        /// <summary>
        /// Factor of load beetwen start tuple and end tuple when cracks are appeared
        /// </summary>
        public double FactorOfCrackAppearance { get; set; }
        /// <summary>
        /// Start force tuple of range where force of cracking is looking for
        /// </summary>
        public IForceTuple StartTuple { get; set; }
        /// <summary>
        /// End force tuple of range where force of cracking is looking for
        /// </summary>
        public IForceTuple EndTuple { get; set; }
        /// <summary>
        /// Force tuple which correspondent to first crack appearence
        /// </summary>
        public IForceTuple TupleOfCrackAppearance { get; set; }
        /// <summary>
        /// General curvature in cracked section
        /// </summary>
        public StrainTuple CrackedStrainTuple { get; set; }
        /// <summary>
        /// Average general curvature with considering of cracking
        /// </summary>
        public StrainTuple ReducedStrainTuple { get; set; }
        /// <summary>
        /// Factor of softening of stifness with considering of cracks
        /// </summary>
        public StrainTuple SofteningFactors { get; set; }
        /// <summary>
        /// Collection of ndms which crack properties looking for
        /// </summary>
        public IEnumerable<INdm> NdmCollection { get; set; }
        /// <summary>
        /// Common softening factor
        /// </summary>
        public double PsiS { get; set; }
        
    }
}
