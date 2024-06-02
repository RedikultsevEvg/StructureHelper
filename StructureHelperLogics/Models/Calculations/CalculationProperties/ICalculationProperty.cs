using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public interface ICalculationProperty
    {
        List<IForceCombination> ForceCombinations { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IAccuracy Accuracy {get;}
    }
}
