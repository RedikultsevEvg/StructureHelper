using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarStressResultLogic : ILogic
    {
        IRebarCrackInputData RebarCrackInputData { get; set; }
        IRebarPrimitive RebarPrimitive { get; set; }

        IRebarStressResult GetRebarStressResult();
    }
}