using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    /// <inheritdoc/>
    public class SteelDiagramAbsoluteProperty : ISteelDiagramAbsoluteProperty
    {
        /// <inheritdoc/>
        public double InitialYoungsModulus { get; set; }
        /// <inheritdoc/>
        public double BaseStrength { get; set; }
        /// <inheritdoc/>
        public double StrainOfProportionality { get; set; }
        /// <inheritdoc/>
        public double StrainOfStartOfYielding { get; set; }
        /// <inheritdoc/>
        public double StrainOfEndOfYielding { get; set; }
        /// <inheritdoc/>
        public double StrainOfUltimateStrength { get; set; }
        /// <inheritdoc/>
        public double StrainOfFracture { get; set; }
        /// <inheritdoc/>
        public double StressOfProportionality { get; set; }
        /// <inheritdoc/>
        public double StressOfUltimateStrength { get; set; }
        /// <inheritdoc/>
        public double StressOfFracture { get; set; }
        public double BaseStrain { get; set; }
    }
}
