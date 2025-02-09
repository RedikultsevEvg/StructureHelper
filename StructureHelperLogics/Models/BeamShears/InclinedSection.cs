using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class InclinedSection : IInclinedSection
    {
        public double EffectiveDepth { get; set; }
        public double WebWidth { get; set; }
        public double StartCoord { get; set; }
        public double EndCoord { get; set; }
    }
}
