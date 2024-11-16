using StructureHelperCommon.Infrastructures.Enums;
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
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IForceTuple FullSLSForces { get; set; }
        double ULSFactor { get; set; }
        double LongTermFactor { get; set; }
    }
}
