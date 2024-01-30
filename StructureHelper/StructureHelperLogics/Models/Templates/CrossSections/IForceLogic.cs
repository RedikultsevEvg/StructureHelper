using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal interface IForceLogic
    {
        IEnumerable<IForceCombinationList> GetCombinationList();
    }
}
