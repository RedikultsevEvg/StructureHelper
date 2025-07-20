using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Properties of stirrups effectiveness
    /// </summary>
    public interface IStirrupEffectiveness
    {
        /// <summary>
        /// Ratio of maximum crack length to effective depth 
        /// </summary>
        double MaxCrackLengthRatio { get; set; }
        /// <summary>
        /// Factor of effectiveness due to non-rectangle shape of stirrup
        /// </summary>
        double StirrupShapeFactor { get; set; }
        /// <summary>
        /// Factor of difference between real and uniform distribution
        /// </summary>
        double StirrupPlacementFactor { get; set; }
        /// <summary>
        /// Minimum ratio of density of stirrup to density of concrete
        /// </summary>
        double MinimumStirrupRatio { get; set; }
    }
}
