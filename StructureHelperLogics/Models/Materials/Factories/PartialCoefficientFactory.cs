using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public static class PartialCoefficientFactory
    {
        public static List<IMaterialSafetyFactor> GetDefaultConcreteSafetyFactors(CodeTypes codeType)
        {
            if (codeType == CodeTypes.SP63_2018) return GetConcreteFactorsSP63_2018();
            else if (codeType == CodeTypes.EuroCode_2_1990) return GetConcreteFactorsEC2_1990();
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + ": " + codeType);
        }

        public static List<IMaterialSafetyFactor> GetDefaultFRSafetyFactors(CodeTypes codeType, MaterialTypes materialType)
        {
            if (codeType == CodeTypes.SP164_2014) return GetFRFactorsSP164_2014(materialType);
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + ": " + codeType);
        }

        private static List<IMaterialSafetyFactor> GetConcreteFactorsEC2_1990()
        {
            List<IMaterialSafetyFactor> factors = new List<IMaterialSafetyFactor>();
            return factors;
        }

        private static List<IMaterialSafetyFactor> GetConcreteFactorsSP63_2018()
        {
            List<IMaterialSafetyFactor> factors = new List<IMaterialSafetyFactor>();
            IMaterialSafetyFactor coefficient;
            coefficient = ConcreteFactorsFactory.GetFactor(ConcreteFactorType.LongTermFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
            coefficient = ConcreteFactorsFactory.GetFactor(ConcreteFactorType.PlainConcreteFactor);
            coefficient.Take = false;
            factors.Add(coefficient);
            coefficient = ConcreteFactorsFactory.GetFactor(ConcreteFactorType.BleedingFactor);
            coefficient.Take = false;
            factors.Add(coefficient);
            return factors;
        }
        private static List<IMaterialSafetyFactor> GetFRFactorsSP164_2014(MaterialTypes materialType)
        {
            List<IMaterialSafetyFactor> factors = new List<IMaterialSafetyFactor>();
            if (materialType == MaterialTypes.CarbonFiber)
            {
                GetCarbonFactors(factors);
            }
            else if (materialType == MaterialTypes.GlassFiber)
            {
                GetGlassFactors(factors);
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + ": " + materialType);
            return factors;
        }
        private static void GetCarbonFactors(List<IMaterialSafetyFactor> factors)
        {
            IMaterialSafetyFactor coefficient;
            coefficient = FRFactorsFactory.GetCarbonFactor(FRFactorType.ConditionFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
            coefficient = FRFactorsFactory.GetCarbonFactor(FRFactorType.LongTermFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
        }
        private static void GetGlassFactors(List<IMaterialSafetyFactor> factors)
        {
            IMaterialSafetyFactor coefficient;
            coefficient = FRFactorsFactory.GetGlassFactor(FRFactorType.ConditionFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
            coefficient = FRFactorsFactory.GetGlassFactor(FRFactorType.LongTermFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
        }
    }
}
