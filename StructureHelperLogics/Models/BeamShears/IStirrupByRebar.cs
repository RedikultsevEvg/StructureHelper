using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IStirrupByRebar : IStirrup
    {
        IReinforcementLibMaterial ReinforcementMaterial { get; set; }
        double LegCount { get; set; }
        double Diameter { get; set; }
        double Step { get; set; }
    }
}
