using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceCalculator : INdmCalculator, IHasPrimitives, IHasForceCombinations
    {
        List<CalcTerms> CalcTermsList { get; }
        List<LimitStates> LimitStatesList { get; }
        ICompressedMember CompressedMember { get; }
        IAccuracy Accuracy { get; }
    }
}