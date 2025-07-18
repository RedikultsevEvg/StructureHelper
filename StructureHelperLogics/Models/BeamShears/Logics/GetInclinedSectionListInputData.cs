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
        public IBeamShearDesignRangeProperty DesignRangeProperty { get; }
        public IGetInclinedSectionLogic? GetInclinedSectionLogic { get; set; }
        public IBeamShearSection BeamShearSection { get; set; }

        public GetInclinedSectionListInputData(IBeamShearDesignRangeProperty designRangeProperty, IBeamShearSection beamShearSection)
        {
            DesignRangeProperty = designRangeProperty;
            BeamShearSection = beamShearSection;
        }
    }
}
