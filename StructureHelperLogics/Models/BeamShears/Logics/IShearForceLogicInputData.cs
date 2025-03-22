using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IShearForceLogicInputData
    {
        IBeamShearAxisAction AxisAction {get;set;}
        IInclinedSection InclinedSection { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
    }
}
