using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.RC
{
    public class AnchorageInputData : IAnchorageInputData
    {
        public double ConcreteStrength { get; set; }
        public double ReinforcementStrength { get; set; }
        public double CrossSectionArea { get; set; }
        public double CrossSectionPerimeter { get; set; }
        public double ReinforcementStress { get; set; }
        public double LappedCountRate { get; set; }
    }
}
