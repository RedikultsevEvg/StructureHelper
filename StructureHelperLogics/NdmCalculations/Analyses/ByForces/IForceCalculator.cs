using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceCalculator : INdmCalculator, IHasPrimitives, IHasForceCombinations
    {
        List<CalcTerms> CalcTermsList { get; }
        double IterationAccuracy { get; set; }
        List<LimitStates> LimitStatesList { get; }
        int MaxIterationCount { get; set; }
    }
}