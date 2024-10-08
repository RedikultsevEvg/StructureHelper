using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHelperMaterial : ISaveable, ICloneable
    {
        IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
        IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
    }
}
