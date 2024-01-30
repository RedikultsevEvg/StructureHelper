using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal interface ISectionCrackedLogic
    {
        IForceTuple Tuple { get; set; }
        IEnumerable<INdm> NdmCollection { get; set; }
        bool IsSectionCracked();
    }
}
