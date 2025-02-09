using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement properties of stirrups
    /// </summary>
    public interface IStirrup : ISaveable, ICloneable
    {
        string? Name { get; set; }
        /// <summary>
        /// Distance from axis of comressed rebar to edge of comressed zone, m 
        /// </summary>
        double CompressedGap { get; set; }
    }
}
