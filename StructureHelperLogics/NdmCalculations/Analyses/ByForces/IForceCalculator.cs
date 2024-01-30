using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceCalculator : ICalculator, IHasPrimitives, IHasForceCombinations
    {
        List<CalcTerms> CalcTermsList { get; }
        List<LimitStates> LimitStatesList { get; }
        ICompressedMember CompressedMember { get; }
        IAccuracy Accuracy { get; set; }
        List<IForceCombinationList> ForceCombinationLists { get;}
    }
}