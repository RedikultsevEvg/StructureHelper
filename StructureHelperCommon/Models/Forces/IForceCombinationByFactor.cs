using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceCombinationByFactor : IForceAction
    {
        IForceTuple FullSLSForces { get; }
        double ULSFactor { get; set; }
        double LongTermFactor { get; set; }
    }
}
