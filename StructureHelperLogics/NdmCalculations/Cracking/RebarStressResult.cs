using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarStressResult : IResult
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
