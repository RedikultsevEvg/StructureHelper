using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperLogics.Models.Analyses
{
    /// <inheritdoc/>
    public class BeamShearAnalysis : IBeamShearAnalysis
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string Tags { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string Comment { get; set; } = string.Empty;
        /// <inheritdoc/>
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);
        /// <inheritdoc/>
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessor();
        public BeamShearAnalysis(Guid id)
        {
            Id = id;
            BeamShear beamShear = new(Guid.NewGuid());
            VersionProcessor.AddVersion(beamShear);
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
