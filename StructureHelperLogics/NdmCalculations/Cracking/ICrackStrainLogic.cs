using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal interface ICrackStrainLogic
    {
        StrainTuple BeforeCrackingTuple { get; set; }
        StrainTuple AfterCrackingTuple { get; set; }
        double SofteningFactor { get; set; }
        StrainTuple GetCrackedStrainTuple();
    }
}
