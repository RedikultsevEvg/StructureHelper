using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface IDeflectionResult
    {
        double UltimateDeflection { get; set; }
        double Curvature { get; set; }
        double Deflection { get; set; }
        bool IsDeflectionLessThanUltimate { get; }
    }
}
