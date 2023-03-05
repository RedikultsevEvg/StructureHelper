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

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface INdmPrimitive : ISaveable, ICloneable
    {
        string? Name { get; set; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        IHeadMaterial? HeadMaterial { get; set; }
        /// <summary>
        /// Flag of triangulation
        /// </summary>
        bool Triangulate { get; set; }
        IStrainTuple UsersPrestrain { get; }
        IStrainTuple AutoPrestrain { get; }
        IVisualProperty VisualProperty {get; }

        IEnumerable<INdm> GetNdms(IMaterial material);
    }
}
