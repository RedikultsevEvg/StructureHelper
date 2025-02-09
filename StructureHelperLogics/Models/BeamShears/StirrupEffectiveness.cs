using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class StirrupEffectiveness : IStirrupEffectiveness
    {
        /// <inheritdoc/>
        public double MaxCrackLengthRatio { get; set; }
        /// <inheritdoc/>
        public double StirrupShapeFactor { get; set; }
        /// <inheritdoc/>
        public double StirrupPlacementFactor { get; set; }
    }
}
