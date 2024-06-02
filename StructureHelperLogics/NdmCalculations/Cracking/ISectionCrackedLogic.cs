using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ISectionCrackedLogic : ILogic
    {
        IForceTuple Tuple { get; set; }
        IEnumerable<INdm> NdmCollection { get; set; }
        bool IsSectionCracked();
    }
}
