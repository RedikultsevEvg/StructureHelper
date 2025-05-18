using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IBeamShearActionResult : IResult
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IBeamShearAction BeamShearAction { get; set; }
        List<IBeamShearSectionLogicResult> SectionResults { get; set; }
    }
}
