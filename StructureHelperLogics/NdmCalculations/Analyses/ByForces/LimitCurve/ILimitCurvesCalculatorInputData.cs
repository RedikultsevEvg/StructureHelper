using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurvesCalculatorInputData : IInputData, ICloneStrategy<ILimitCurvesCalculatorInputData>
    {
        List<CalcTerms> CalcTerms { get; }
        List<LimitStates> LimitStates { get; }
        int PointCount { get; set; }
        List<PredicateEntry> PredicateEntries { get; }
        List<NamedCollection<INdmPrimitive>> PrimitiveSeries { get; }
        ISurroundData SurroundData { get; set; }
    }
}