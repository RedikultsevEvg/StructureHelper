using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class InclinedSection : IInclinedSection
    {
        public double StartCoord { get; set; }
        public double EndCoord { get; set; }
        public double EffectiveDepth { get; set; }
    }
}
