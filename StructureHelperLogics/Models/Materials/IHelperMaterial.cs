using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHelperMaterial : ICloneable
    {
        IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
    }
}
