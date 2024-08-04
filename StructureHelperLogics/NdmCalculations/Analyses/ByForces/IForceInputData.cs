using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceInputData : IInputData, IHasPrimitives, IHasForceCombinations
    {
        IAccuracy Accuracy { get; set; }
        List<CalcTerms> CalcTermsList { get; }
        ICompressedMember CompressedMember { get; set; }
        List<IForceCombinationList> ForceCombinationLists { get; set; }
        List<LimitStates> LimitStatesList { get; }
    }
}