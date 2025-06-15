using StructureHelperCommon.Models.Materials.Libraries;

namespace DataAccess.DTOs
{
    public class MaterialSafetyFactorsFromDTOLogic : IMaterialSafetyFactorDTOLogic
    {
        public IMaterialSafetyFactor GetNewSafetyFactorByOld(IMaterialSafetyFactor safetyFactor)
        {
            MaterialSafetyFactor newItem = new(safetyFactor.Id);
            return newItem;
        }
        public IMaterialPartialFactor GetNewPartialFactorByOld(IMaterialPartialFactor partialFactor)
        {
            MaterialPartialFactor newItem = new(partialFactor.Id);
            return newItem;
        }

    }
}
