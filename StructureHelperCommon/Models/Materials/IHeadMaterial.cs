using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Windows.Media;

namespace StructureHelper.Models.Materials
{
    public interface IHeadMaterial : ISaveable, ISelfCloneStrategy<IHeadMaterial>
    {
        string Name { get; set; }
        Color Color { get; set; }
        IHelperMaterial HelperMaterial { get; set; }

        IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            => HelperMaterial.GetLoaderMaterial(limitState, calcTerm);

        IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
            => HelperMaterial.GetCrackedLoaderMaterial(limitState, calcTerm);
    }
}
