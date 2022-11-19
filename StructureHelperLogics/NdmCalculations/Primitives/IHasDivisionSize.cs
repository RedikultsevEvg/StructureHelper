using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal interface IHasDivisionSize
    {
        double NdmMaxSize { get; set; }
        int NdmMinDivision { get; set; }
    }
}
