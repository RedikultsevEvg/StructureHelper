using LoaderCalculator.Data.Matrix;
using StructureHelperLogics.Infrastructures.CommonEnums;
using System;
using System.Collections.Generic;
using System.Text;

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
