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
        public double ActualFactor { get; set; }
        public IForceTuple ActualTuple { get; set; }
    }
}
