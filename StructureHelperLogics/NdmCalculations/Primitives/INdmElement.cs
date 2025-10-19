using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface INdmElement : ISaveable, ICloneable
    {
        /// <summary>
        /// Material of primitive
        /// </summary>
        IHeadMaterial? HeadMaterial { get; set; }
        /// <summary>
        /// Flag of triangulation
        /// </summary>
        bool Triangulate { get; set; }
        /// <summary>
        /// Prestrain assigned from user
        /// </summary>
        IForceTuple UsersPrestrain { get; }
        /// <summary>
        /// Prestrain assigned from calculations
        /// </summary>
        IForceTuple AutoPrestrain { get; }
    }
}
