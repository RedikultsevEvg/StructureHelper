using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculator : ILogicCalculator
    {
        ICurvatureForceCalculatorInputData InputData { get; set; }
    }
}
