using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public class CalculationProperty : ICalculationProperty
    {
        public List<IForceCombination> ForceCombinations { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IAccuracy Accuracy { get; }

        public CalculationProperty()
        {
            ForceCombinations = new List<IForceCombination>
            {
                new ForceCombination()
            };
            LimitState = LimitStates.ULS;
            CalcTerm = CalcTerms.ShortTerm;
            Accuracy = new Accuracy () { IterationAccuracy = 0.001d, MaxIterationCount = 100};
        }
    }
}
