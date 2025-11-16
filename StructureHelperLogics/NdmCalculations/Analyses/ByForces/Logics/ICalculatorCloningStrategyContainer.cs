using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ICalculatorCloningStrategyContainer
    {
        IUpdateStrategy<IForceCalculator> ForceCalculatorStrategy { get; }
        IUpdateStrategy<ICrackCalculator> CrackCalculatorStrategy { get; }
        IUpdateStrategy<ILimitCurvesCalculator> LimitCurvesCalculatorStrategy { get; }
        IUpdateStrategy<IValueDiagramCalculator> ValueDiagramCalculatorStrategy { get; }
    }
}
