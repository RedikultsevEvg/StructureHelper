using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    internal interface IForceCombinationFromFile : IForceAction
    {
        List<IForceFileProperty> ForceFiles { get; set; }
    }
}
