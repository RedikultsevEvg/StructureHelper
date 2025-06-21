using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackResult : IResult
    {
        List<ITupleCrackResult> TupleResults { get; set; }
    }
}