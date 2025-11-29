using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class DeflectionResult : IDeflectionResult
    {
        public double UltimateDeflection { get; set; } = 0.0;
        public double Curvature { get; set; } = 0.0;
        public double Deflection { get; set; } = 0.0;

        public bool IsDeflectionLessThanUltimate => Math.Abs(Deflection) <= Math.Abs(UltimateDeflection);
    }
}
