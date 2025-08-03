using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement properties for uniformly distributed stirrups
    /// </summary>
    public interface IStirrupByRebar : IStirrup, IHasStartEndCoordinate
    {
        /// <summary>
        /// Material of stirrups
        /// </summary>
        IReinforcementLibMaterial Material { get; set; }
        /// <summary>
        /// True if hoop is spiral one
        /// </summary>
        bool IsSpiral { get; set; }
        /// <summary>
        /// Count of legs of stirrup in specific cross-section
        /// </summary>
        double LegCount { get; set; }
        /// <summary>
        /// Diameter of stirrup, m
        /// </summary>
        double Diameter { get; set; }
        /// <summary>
        /// Step of uniformly distibuted stirrup along axis of beam, m
        /// </summary>
        double Spacing { get; set; }
    }
}
