using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetInclinedSectionListInputData
    {
        int StepCount { get; set; }
        double MaxInclinedSectionLegthFactor { get; set; }
        IGetInclinedSectionLogic? GetInclinedSectionLogic { get; set; }
        IBeamShearSection BeamShearSection { get; set; }
    }
}
