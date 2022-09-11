using LoaderCalculator.Data.Matrix;
using StructureHelperLogics.Infrastructures.CommonEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public class CalculationProperty : ICalculationProperty
    {
        public List<IForceCombination> ForceCombinations { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IIterationProperty IterationProperty { get; }

        public CalculationProperty()
        {
            ForceCombinations = new List<IForceCombination>
            {
                new ForceCombination()
            };
            LimitState = LimitStates.Collapse;
            CalcTerm = CalcTerms.ShortTerm;
            IterationProperty = new IterationProperty() { Accuracy = 0.001d, MaxIterationCount = 100};
        }
    }
}
