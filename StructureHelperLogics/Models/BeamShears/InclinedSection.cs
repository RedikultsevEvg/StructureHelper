using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class InclinedSection : IInclinedSection
    {
        /// <inheritdoc/>
        public double FullDepth { get; set; }
        /// <inheritdoc/>
        public double EffectiveDepth { get; set; }
        /// <inheritdoc/>
        public double WebWidth { get; set; }
        /// <inheritdoc/>
        public double StartCoord { get; set; }
        /// <inheritdoc/>
        public double EndCoord { get; set; }
        public double ConcreteCompressionStrength { get; set; }
        public double ConcreteTensionStrength { get; set; }
    }
}
