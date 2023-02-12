using LoaderCalculator.Data.Matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public class ForceCombination : IForceCombination
    {
        public IForceMatrix ForceMatrix { get; private set; }
        public bool TakeInCalculate { get; set; }

        public ForceCombination()
        {
            ForceMatrix = new ForceMatrix() { Mx = 0d, My = 0d, Nz = 0d};
            TakeInCalculate = true;
        }
    }
}
