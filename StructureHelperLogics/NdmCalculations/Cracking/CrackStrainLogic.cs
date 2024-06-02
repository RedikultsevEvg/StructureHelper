using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class CrackStrainLogic : ICrackStrainLogic
    {
        public StrainTuple BeforeCrackingTuple { get; set; }
        public StrainTuple AfterCrackingTuple { get; set; }
        public double SofteningFactor { get; set; }

        public StrainTuple GetCrackedStrainTuple()
        {
            var strainTuple = ForceTupleService.InterpolateTuples(AfterCrackingTuple, BeforeCrackingTuple, SofteningFactor) as StrainTuple;
            return strainTuple;
        }
    }
}
