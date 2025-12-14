using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces.Logics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public enum SteelDiagramPropertyType
    {
        S245 = 0, //S245, S255
        S345 = 1, //S345, S345K, S355, S355-1, S355-P
        S390 = 2, //S390, S390-1
        S440 = 3, //S440
        S550 = 4, //S550, S590
    }
    public static class SteelDiagramPropertyFactory
    {
        public static ISteelDiagramRelativeProperty GetProperty(SteelDiagramPropertyType type)
        {
            if (type == SteelDiagramPropertyType.S245)
            {
                SteelDiagramRelativeProperty property = new()
                {
                    StrainOfProportionality = 0.8,
                    StrainOfStartOfYielding = 1.7,
                    StrainOfEndOfYielding = 14.0,
                    StrainOfUltimateStrength = 141.6,
                    StrainOfFracture = 251,
                    StressOfUltimateStrength = 1.653,
                    StressOfFracture = 1.35
                };
                return property;
            }
            else if (type == SteelDiagramPropertyType.S345)
            {
                SteelDiagramRelativeProperty property = new()
                {
                    StrainOfProportionality = 0.8,
                    StrainOfStartOfYielding = 1.7,
                    StrainOfEndOfYielding = 16.0,
                    StrainOfUltimateStrength = 88.3,
                    StrainOfFracture = 153,
                    StressOfUltimateStrength = 1.415,
                    StressOfFracture = 1.26
                };
                return property;
            }
            else if (type == SteelDiagramPropertyType.S390)
            {
                SteelDiagramRelativeProperty property = new()
                {
                    StrainOfProportionality = 0.9,
                    StrainOfStartOfYielding = 1.7,
                    StrainOfEndOfYielding = 17.0,
                    StrainOfUltimateStrength = 67.1,
                    StrainOfFracture = 115,
                    StressOfUltimateStrength = 1.345,
                    StressOfFracture = 1.23
                };
                return property;
            }
            else if (type == SteelDiagramPropertyType.S440)
            {
                SteelDiagramRelativeProperty property = new()
                {
                    StrainOfProportionality = 0.9,
                    StrainOfStartOfYielding = 1.7,
                    StrainOfEndOfYielding = 17.0,
                    StrainOfUltimateStrength = 49.6,
                    StrainOfFracture = 87.2,
                    StressOfUltimateStrength = 1.33,
                    StressOfFracture = 1.20
                };
                return property;
            }
            else if (type == SteelDiagramPropertyType.S550)
            {
                SteelDiagramRelativeProperty property = new()
                {
                    StrainOfProportionality = 0.9,
                    StrainOfStartOfYielding = 1.7,
                    StrainOfEndOfYielding = 18.0,
                    StrainOfUltimateStrength = 26.2,
                    StrainOfFracture = 51.1,
                    StressOfUltimateStrength = 1.16,
                    StressOfFracture = 1.10
                };
                return property;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(type));
            }
        }
    }
}
