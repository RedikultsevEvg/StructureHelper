using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class GetInclinedSectionListInputData : IGetInclinedSectionListInputData
    {
        public int StepCount { get; set; } = 50;
        public double MaxInclinedSectionLegthFactor { get; set; } = 3d;
        public IGetInclinedSectionLogic? GetInclinedSectionLogic { get; set; }
        public IBeamShearSection BeamShearSection { get; set; }

        public GetInclinedSectionListInputData(IBeamShearSection beamShearSection)
        {
            BeamShearSection = beamShearSection;
        }
    }
}
