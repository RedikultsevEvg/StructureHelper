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
        IForceTuple BeforeCrackingTuple { get; set; }
        IForceTuple AfterCrackingTuple { get; set; }
        double SofteningFactor { get; set; }
        IForceTuple GetCrackedStrainTuple();
    }
}
