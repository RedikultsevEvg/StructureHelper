using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IFactoredForceTuple
    {
        IForceTuple ForceTuple { get; set; }
        IFactoredCombinationProperty CombinationProperty { get; set; }
    }
}
