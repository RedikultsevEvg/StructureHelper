using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.RC
{
    public interface IAnchorageInputData
    {
        double ConcreteStrength { get; set; }
        double ReinforcementStrength { get; set; }
        double FactorEta1 { get; set; }
        double CrossSectionArea { get; set; }
        double CrossSectionPerimeter { get; set; }
        double ReinforcementStress { get; set; }
        double LappedCountRate { get; set; }
        bool IsPrestressed { get; set; }

    }
}
