using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    /// <summary>
    /// Implements properies of cross-section of reinforcement bar
    /// </summary>
    public interface IRebarSection : ISaveable, ICloneable
    {
        /// <summary>
        /// Material of rebar
        /// </summary>
        IReinforcementLibMaterial Material { get; set; }
        /// <summary>
        /// Diameter of rebar
        /// </summary>
        double Diameter { get; set; }
    }
}
