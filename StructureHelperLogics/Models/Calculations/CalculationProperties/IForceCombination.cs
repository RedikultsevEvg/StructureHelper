using LoaderCalculator.Data.Matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public interface IForceCombination
    {
        IForceMatrix ForceMatrix { get; }
        bool TakeInCalculate { get; set; }
    }
}
