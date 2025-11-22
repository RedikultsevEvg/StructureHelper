using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureCalculator : ICalculator
    {
        ICurvatureCalculatorInputData InputData { get; set; }
    }
}
