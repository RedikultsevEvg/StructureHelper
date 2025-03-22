using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class ShearForceLogicInputData : IShearForceLogicInputData
    {
        public IBeamShearAxisAction AxisAction { get; set; }
        public IInclinedSection InclinedSection { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
    }
}
