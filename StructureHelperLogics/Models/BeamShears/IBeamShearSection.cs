using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Properties of RC cross-section for shear strength of beam
    /// </summary>
    public interface IBeamShearSection : ISaveable, ICloneable
    {
        string? Name { get; set; }
        /// <summary>
        /// Concrete of cross-section
        /// </summary>
        IConcreteLibMaterial Material { get; set; }
        /// <summary>
        /// Shape of cross-section
        /// </summary>
        IShape Shape { get; set; }
        /// <summary>
        /// Distance from edge of tension zone to center of the nearest reinforcement bar
        /// </summary>
        double CenterCover { get; set; }
    }
}
