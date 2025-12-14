using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class SteelDiagramRelativeProperty : ISteelDiagramRelativeProperty
    {
        public double StrainOfProportionality { get; set; }
        public double StrainOfStartOfYielding { get; set; }
        public double StrainOfEndOfYielding { get; set; }
        public double StrainOfUltimateStrength { get; set; }
        public double StressOfUltimateStrength { get; set; }
        public double StrainOfFracture { get; set; }
        public double StressOfFracture { get; set; }
    }
}
