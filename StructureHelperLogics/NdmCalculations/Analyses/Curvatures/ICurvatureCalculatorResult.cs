using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureCalculatorResult : IResult
    {
        ICurvatureCalculatorInputData InputData { get; set; }
        List<ICurvatureForceCalculatorResult> ForceCalculatorResults { get; }
    }
}
