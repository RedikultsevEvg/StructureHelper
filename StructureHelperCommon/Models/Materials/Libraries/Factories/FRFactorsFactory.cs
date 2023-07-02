using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public enum FRFactorType
    {
        ConditionFactor,
        LongTermFactor,
    }
    public static class FRFactorsFactory
    {
        const string gammaf1Name = "Gamma_f1";
        const string gammaf1Description = "Coefficient for considering environment";
        const string gammaf3Name = "Gamma_f3";
        const string gammaf3Description = "Coefficient for considering long term calculations";
        public static IMaterialSafetyFactor GetCarbonFactor(FRFactorType factorType)
        {
            if (factorType == FRFactorType.LongTermFactor) { return LongTerm(1d / 1.2d,0.8d); }
            else if (factorType == FRFactorType.ConditionFactor) { return Condition(gammaf1Name, gammaf1Description , 0.9d); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }
        public static IMaterialSafetyFactor GetGlassFactor(FRFactorType factorType)
        {
            if (factorType == FRFactorType.LongTermFactor) { return LongTerm(1d / 1.8d, 0.3d); }
            else if (factorType == FRFactorType.ConditionFactor) { return Condition(gammaf1Name, gammaf1Description, 0.7d); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }

        private static IMaterialSafetyFactor Condition(string name, string description, double value)
        {
            IMaterialSafetyFactor safetyFactor = new MaterialSafetyFactor()
            {
                Name = name,
                Description = description,
            };
            IMaterialPartialFactor partialFactor;
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Tension,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = value
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = value
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Tension,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = value
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = value
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            return safetyFactor;
        }

        private static IMaterialSafetyFactor LongTerm(double shortValue, double longValue)
        {
            IMaterialSafetyFactor safetyFactor = new MaterialSafetyFactor()
            {
                Name = gammaf3Name,
                Description = gammaf3Description,
            };
            IMaterialPartialFactor partialFactor;
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Tension,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = shortValue
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = shortValue
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Tension,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = longValue
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = longValue
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            return safetyFactor;
        }
    }
}
