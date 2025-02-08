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
    public class BeamShearAnalysis : IBeamShearAnalysis
    {

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.FromRgb(128, 0, 0);
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
