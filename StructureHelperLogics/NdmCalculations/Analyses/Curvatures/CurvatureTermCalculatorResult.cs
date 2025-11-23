using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermCalculatorResult : ICurvatureTermCalculatorResult
    {
        public ICurvatureTermCalculatorInputData InputData { get; set; }
        public IForceTuple CurvatureValues { get; set; }
        public IForceTuple Deflections { get; set; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
