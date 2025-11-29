using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureSectionResult : IResult
    {
        ICurvatureForceCalculatorResult InputData { get; set; }
        ICurvatureTermResult LongTermResult { get; set; }
        ICurvatureTermResult ShortTermResult { get; set; }
    }
}
