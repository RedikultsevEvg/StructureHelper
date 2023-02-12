using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public enum FactorType
    {
        LongTermFactor,
        BleedingFactor,
        PlainConcreteFactor
    }

    public static class ConcreteFactorsFactory
    {
        public static IMaterialSafetyFactor GetFactor(FactorType factorType)
        {
            if (factorType == FactorType.LongTermFactor) { return LongTerm(); }
            else if (factorType == FactorType.BleedingFactor) { return Bleeding(); }
            else if (factorType == FactorType.PlainConcreteFactor) { return PlainConcrete(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
        }

        private static IMaterialSafetyFactor LongTerm()
        {
            IMaterialSafetyFactor safetyFactor = new MaterialSafetyFactor()
            {
                Name = "Gamma_b1",
                Description = "Coefficient for considering long term calculations",
            };
            IMaterialPartialFactor partialFactor;
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Tension,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.9d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.9d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            return safetyFactor;
        }

        private static IMaterialSafetyFactor Bleeding()
        {
            IMaterialSafetyFactor safetyFactor = new MaterialSafetyFactor()
            {
                Name = "Gamma_b3",
                Description = "Coefficient for considering bleeding in vertical placement conditionals",
            };
            IMaterialPartialFactor partialFactor;
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.85d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.85d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            return safetyFactor;
        }

        private static IMaterialSafetyFactor PlainConcrete()
        {
            IMaterialSafetyFactor safetyFactor = new MaterialSafetyFactor()
            {
                Name = "Gamma_b2",
                Description = "Coefficient for plain concrete structures",
            };
            IMaterialPartialFactor partialFactor;
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.ShortTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.9d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            partialFactor = new MaterialPartialFactor
            {
                StressState = StressStates.Compression,
                CalcTerm = CalcTerms.LongTerm,
                LimitState = LimitStates.ULS,
                FactorValue = 0.9d
            };
            safetyFactor.PartialFactors.Add(partialFactor);
            return safetyFactor;
        }


    }
}
