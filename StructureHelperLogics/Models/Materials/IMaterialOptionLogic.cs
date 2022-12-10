using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperLogics.Models.Materials
{
    public interface IMaterialOptionLogic
    {
        LCMB.IMaterialOptions SetMaterialOptions(ILibMaterialEntity materialEntity, LimitStates limitState, CalcTerms calcTerm);
    }
}