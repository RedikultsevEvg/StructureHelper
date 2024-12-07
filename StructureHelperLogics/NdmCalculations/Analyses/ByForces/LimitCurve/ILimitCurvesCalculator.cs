using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurvesCalculator : ISaveable, ICalculator, IHasActionByResult
    {
        ILimitCurvesCalculatorInputData InputData { get; set; }
        string Name { get; set; }
    }
}