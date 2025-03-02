using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Properties of concrete cross-section for shear strength of beam
    /// </summary>
    public interface IBeamShearSection : ISaveable, ICloneable
    {
        string? Name { get; set; }
        /// <summary>
        /// Concrete of cross-section
        /// </summary>
        IMaterialStrength MaterialStrength { get;}
        /// <summary>
        /// Shape of cross-section
        /// </summary>
        IShape Shape { get; }
        /// <summary>
        /// Distance from edge of tension zone to center of the nearest reinforcement bar
        /// </summary>
        double CenterCover { get; set; }
    }
}
