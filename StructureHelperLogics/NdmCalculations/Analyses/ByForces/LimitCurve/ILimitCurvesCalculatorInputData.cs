using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurvesCalculatorInputData : IInputData, ICloneable
    {
        List<CalcTerms> CalcTerms { get; }
        List<LimitStates> LimitStates { get; }
        int PointCount { get; set; }
        List<PredicateEntry> PredicateEntries { get; }
        List<NamedCollection<INdmPrimitive>> PrimitiveSeries { get; }
        ISurroundData SurroundData { get; set; }
    }
}