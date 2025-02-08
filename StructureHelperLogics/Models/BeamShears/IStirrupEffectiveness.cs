using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IStirrupEffectiveness
    {
        double MaxCrackLengthFactor { get; set; }
        double StirrupShapeFactor { get; set; }
        double StirrupPlacementFactor { get; set; }
    }
}
