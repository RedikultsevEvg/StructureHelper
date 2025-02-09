using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class SectionEffectiveness : ISectionEffectiveness
    {
        /// <inheritdoc/>
        public double BaseShapeFactor { get; set; }
        /// <inheritdoc/>
        public double MaxCrackLengthRatio { get; set; }
        /// <inheritdoc/>
        public double MinCrackLengthRatio { get; set; }
        /// <inheritdoc/>
        public double ShapeFactor { get; set; }
    }
}
