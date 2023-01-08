using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
            if (codeType == CodeTypes.SP63_13330_2018) return GetConcreteFactorsSP63_2018();
            else if (codeType == CodeTypes.EuroCode_2_1990) return GetConcreteFactorsEC2_1990();
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
            coefficient = ConcreteFactorsFactory.GetFactor(FactorType.LongTermFactor);
            coefficient.Take = true;
            factors.Add(coefficient);
            coefficient = ConcreteFactorsFactory.GetFactor(FactorType.PlainConcreteFactor);
            coefficient.Take = false;
            factors.Add(coefficient);
            coefficient = ConcreteFactorsFactory.GetFactor(FactorType.BleedingFactor);
            coefficient.Take = false;
            factors.Add(coefficient);
            return factors;
        }
    }
}
