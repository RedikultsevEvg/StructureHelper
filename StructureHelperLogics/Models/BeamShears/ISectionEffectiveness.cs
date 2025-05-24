using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements properties of concrete cross-section effectiveness for shear
    /// </summary>
    public interface ISectionEffectiveness
    {
        /// <summary>
        /// Shape factor of shear strength of base form, dimensionless
        /// </summary>
        double BaseShapeFactor { get; set; }
        /// <summary>
        /// Maximum ratio of crack length to effective depth
        /// </summary>
        double MaxCrackLengthRatio { get; set; }
        /// <summary>
        /// Maximum ratio of crack length to effective depth
        /// </summary>
        double MinCrackLengthRatio { get; set; }
        /// <summary>
        /// Shape factor of shear strength of specific form, dimensionless
        /// </summary>
        double ShapeFactor { get; set; }
    }
}
