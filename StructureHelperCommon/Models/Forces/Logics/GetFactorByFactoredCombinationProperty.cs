using StructureHelperCommon.Infrastructures.Enums;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class GetFactorByFactoredCombinationProperty : IGetLoadFactor
    {
        private const double defaultSLSfactor = 1d;
        private const double defaultShortTermFactor = 1d;

        public IFactoredCombinationProperty CombinationProperty { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public double GetFactor()
        {
            double stateFactor = CombinationProperty.LimitState is LimitStates.SLS ? defaultSLSfactor : (1d / CombinationProperty.ULSFactor);
            double termFactor = CombinationProperty.CalcTerm is CalcTerms.ShortTerm ? defaultShortTermFactor : (1d / CombinationProperty.LongTermFactor);
            double shortSlsFactor = stateFactor * termFactor;
            stateFactor = LimitState is LimitStates.SLS ? 1d : CombinationProperty.ULSFactor;
            termFactor = CalcTerm is CalcTerms.ShortTerm ? 1d : CombinationProperty.LongTermFactor;
            double factor = shortSlsFactor * stateFactor * termFactor;
            return factor;
        }

    }
}
