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
    public class RebarStressResult : IRebarStressResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }

        public StrainTuple StrainTuple { get; set; }

        public double RebarStress { get; set; }

        public double RebarStrain { get; set; }

        public double ConcreteStrain { get; set; }
    }
}
