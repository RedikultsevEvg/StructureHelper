using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ISteelDiagramRelativeProperty
    {
        double StrainOfProportionality { get; set; }
        double StrainOfStartOfYielding { get; set; }
        double StrainOfEndOfYielding { get; set; }
        double StrainOfUltimateStrength { get; set; }
        double StressOfUltimateStrength { get; set; }
        double StrainOfFracture { get; set; }
        double StressOfFracture { get; set; }
    }
}
