using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class StirrupByUniformRebar : IStirrupByUniformRebar
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        public string? Name { get; set; }
        /// <inheritdoc/>
        public IReinforcementLibMaterial Material { get; set; }
        /// <inheritdoc/>
        public double LegCount { get; set; } = 2;
        /// <inheritdoc/>
        public double Diameter { get; set; } = 0.008;
        /// <inheritdoc/>
        public double Step { get; set; } = 0.1;
        /// <inheritdoc/>
        public double CompressedGap { get; set; } = 0;


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
