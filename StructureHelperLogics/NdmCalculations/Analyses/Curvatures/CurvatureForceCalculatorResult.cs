using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureForceCalculatorResult : ICurvatureForceCalculatorResult
    {
        public ICurvatureForceCalculatorInputData InputData { get; set; }
        public ICurvatureTermCalculatorResult LongTermResult { get; set; }
        public ICurvatureTermCalculatorResult ShortTermResult { get; set; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
