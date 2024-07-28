using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ITupleRebarsCrackSolver : ILogic
    {
        string Description { get; }
        TupleCrackInputData InputData { get; set; }
        bool IsResultValid { get; }
        double LongLength { get; set; }
        IEnumerable<RebarPrimitive> Rebars { get; set; }
        List<RebarCrackResult> Result { get; }
        double ShortLength { get; set; }

        void Run();
    }
}