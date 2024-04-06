using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthTupleResult
    {
        public IForceTuple? ForceTuple { get; set; }
        public double CrackWidth { get; set; }
        public double RebarStrain { get; set; }
        public double ConcreteStrain { get; set; }
    }
}
