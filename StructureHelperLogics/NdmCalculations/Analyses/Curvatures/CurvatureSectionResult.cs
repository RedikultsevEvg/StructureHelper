using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureSectionResult : ICurvatureSectionResult
    {
        public ICurvatureForceCalculatorResult InputData { get; set; }
        public ICurvatureTermResult LongTermResult { get; set; }
        public ICurvatureTermResult ShortTermResult { get; set; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
