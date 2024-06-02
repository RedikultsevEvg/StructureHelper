using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthTupleResult : ICrackWidthTupleResult
    {
        public double CrackWidth { get; set; }
        public double UltimateCrackWidth { get; set; }
        public bool IsCrackLessThanUltimate => CrackWidth <= UltimateCrackWidth;
    }
}
