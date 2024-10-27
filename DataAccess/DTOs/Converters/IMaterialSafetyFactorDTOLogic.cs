using StructureHelperCommon.Models.Materials.Libraries;

namespace DataAccess.DTOs
{
    public interface IMaterialSafetyFactorDTOLogic
    {
        IMaterialPartialFactor GetNewPartialFactorByOld(IMaterialPartialFactor partialFactor);
        IMaterialSafetyFactor GetNewSafetyFactorByOld(IMaterialSafetyFactor safetyFactor);
    }
}