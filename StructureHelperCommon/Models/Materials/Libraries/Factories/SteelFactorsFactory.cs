using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries.Factories
{
    public enum SteelFactorTypes
    {
        WorkCondition = 0,
    }
    public class SteelFactorsFactory
    {
        public static IMaterialSafetyFactor GetFactor(SteelFactorTypes factorType)
        {
            if (factorType == SteelFactorTypes.WorkCondition) { return WorkCondition(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }

        private static IMaterialSafetyFactor WorkCondition()
        {
            throw new NotImplementedException();
        }
    }
}
