using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class MaterialSafetyFactorToDTOLogic : IMaterialSafetyFactorDTOLogic
    {
        public IMaterialSafetyFactor GetNewSafetyFactorByOld(IMaterialSafetyFactor safetyFactor)
        {
            MaterialSafetyFactorDTO newSafetyFactor = new()
            {
                Id = safetyFactor.Id
            };
            return newSafetyFactor;
        }

        public IMaterialPartialFactor GetNewPartialFactorByOld(IMaterialPartialFactor partialFactor)
        {
            MaterialPartialFactorDTO newPartialFactor = new()
            {
                Id = partialFactor.Id
            };
            return newPartialFactor;
        }
    }
}
