using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Geometrical primitive which generates ndm elemtntary part
    /// </summary>
    public interface INdmPrimitive : ISaveable, IHasCenter2D, ICloneable
    {
        /// <summary>
        /// Name of primitive
        /// </summary>
        string? Name { get; set; }
        IShape Shape { get; }
        /// <summary>
        /// Base properties of primitive
        /// </summary>
        INdmElement NdmElement { get;}
        /// <summary>
        /// Host cross-section for primitive
        /// </summary>
        ICrossSection? CrossSection { get; set; }
        /// <summary>
        /// Visual settings
        /// </summary>
        IVisualProperty VisualProperty {get; }

        IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions);
        List<INamedAreaPoint> GetValuePoints();
    }
}
