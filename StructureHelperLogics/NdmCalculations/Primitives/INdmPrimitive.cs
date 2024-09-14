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
