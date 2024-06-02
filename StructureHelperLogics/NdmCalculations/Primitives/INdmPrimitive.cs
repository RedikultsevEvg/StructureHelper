using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;
using System.Collections;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;
using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperCommon.Models.Parameters;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Geometrical primitive which generates ndm elemtntary part
    /// </summary>
    public interface INdmPrimitive : ISaveable, ICloneable
    {
        /// <summary>
        /// Name of primitive
        /// </summary>
        string? Name { get; set; }
        /// <summary>
        /// Base point of primitive
        /// </summary>
        IPoint2D Center { get; }
        /// <summary>
        /// Host cross-section for primitive
        /// </summary>
        ICrossSection? CrossSection { get; set; }
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
        StrainTuple UsersPrestrain { get; }
        /// <summary>
        /// Prestrain assigned from calculations
        /// </summary>
        StrainTuple AutoPrestrain { get; }
        /// <summary>
        /// Visual settings
        /// </summary>
        IVisualProperty VisualProperty {get; }

        IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions);
        List<INamedAreaPoint> GetValuePoints();
    }
}
