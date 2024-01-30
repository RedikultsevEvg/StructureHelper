using StructureHelperCommon.Infrastructures.Enums;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public interface ICalculationProperty
    {
        List<IForceCombination> ForceCombinations { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IIterationProperty IterationProperty {get;}
    }
}
