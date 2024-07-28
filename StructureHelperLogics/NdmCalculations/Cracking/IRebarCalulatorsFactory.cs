using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarCalulatorsFactory : ILogic
    {
        TupleCrackInputData InputData { get; set; }
        double LongLength { get; set; }
        IEnumerable<RebarPrimitive> Rebars { get; set; }
        double ShortLength { get; set; }

        List<IRebarCrackCalculator> GetCalculators();
    }
}