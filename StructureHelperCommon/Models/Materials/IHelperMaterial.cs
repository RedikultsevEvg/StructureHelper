using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Materials
{
    public interface IHelperMaterial : ISaveable, IHasSafetyFactors, ISelfCloneStrategy<IHelperMaterial>
    {
        IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
        IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm);
    }
}
