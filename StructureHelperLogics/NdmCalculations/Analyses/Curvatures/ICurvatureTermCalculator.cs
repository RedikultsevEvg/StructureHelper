using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermCalculator : ILogicCalculator
    {
        ICurvatureForceCalculatorInputData InputData { get; set; }
    }
}
