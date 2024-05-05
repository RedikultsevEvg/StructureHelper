using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IGetTupleInputDatasLogic : ILogic, IHasPrimitives, IHasForceCombinations
    {
        LimitStates LimitState { get; set; }
        CalcTerms LongTerm { get; set; }
        CalcTerms ShortTerm { get; set; }
        List<TupleCrackInputData> GetTupleInputDatas();
    }
}