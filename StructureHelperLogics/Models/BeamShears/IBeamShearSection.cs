using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Properties of RC cross-section for shear strength of beam
    /// </summary>
    public interface IBeamShearSection : ISaveable, ICloneable, IHasVisualProperty
    {
        string? Name { get; set; }
        /// <summary>
        /// Concrete of cross-section
        /// </summary>
        IConcreteLibMaterial ConcreteMaterial { get; set; }
        /// <summary>
        /// Shape of cross-section
        /// </summary>
        IShape Shape { get; set; }
        /// <summary>
        /// Distance from edge of tension zone to center of the nearest reinforcement bar
        /// </summary>
        double CenterCover { get; set; }
        /// <summary>
        /// Area of reinforcement in tension zone, m^2
        /// </summary>
        double ReinforcementArea { get; set; }
        /// <summary>
        /// Material of reinforcement in tension zone
        /// </summary>
        IReinforcementLibMaterial ReinforcementMaterial { get; set; }
    }
}
