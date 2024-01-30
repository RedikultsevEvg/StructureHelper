using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using LCM = LoaderCalculator.Data.Materials;
using LCMB = LoaderCalculator.Data.Materials.MaterialBuilders;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialOptionLogic
    {
        void SetMaterialOptions(LCMB.IMaterialOptions materialOptions);
    }
}