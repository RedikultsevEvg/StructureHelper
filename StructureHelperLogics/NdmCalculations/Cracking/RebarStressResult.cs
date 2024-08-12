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
    /// Result of calculation of stress and strain in rebar
    /// </summary>
    public class RebarStressResult : IResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }
        /// <summary>
        /// Strain tuple which stress and strain is obtained for
        /// </summary>
        public StrainTuple StrainTuple { get; set; } 
        /// <summary>
        /// Stress in rebar, Pa
        /// </summary>
        public double RebarStress { get; set; }
        /// <summary>
        /// Strain in rebar, dimensionless
        /// </summary>
        public double RebarStrain { get; set; }
        /// <summary>
        /// Strain in fake concrete ndm-part which rounds rebas and locatade at axis of rebar (refrence strain in concrete)
        /// </summary>
        public double ConcreteStrain { get; set; }
    }
}
