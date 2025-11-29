using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermResult : ICurvatureTermResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public IDeflectionResult DeflectionMx { get; set; } = new DeflectionResult();
        public IDeflectionResult DeflectionMy { get; set; } = new DeflectionResult();
        public IDeflectionResult DeflectionNz { get; set; } = new DeflectionResult();
    }
}
