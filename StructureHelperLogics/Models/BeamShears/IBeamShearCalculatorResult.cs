using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements group of results for beam shear calculator
    /// </summary>
    public interface IBeamShearCalculatorResult : IResult
    {
        IBeamShearCalculatorInputData InputData {get;set;}
        List<IBeamShearActionResult> ActionResults { get; set; }
    }
}
