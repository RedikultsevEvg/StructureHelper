using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHelperMaterial : ICloneable
    {
        IPrimitiveMaterial GetPrimitiveMaterial();
    }
}
