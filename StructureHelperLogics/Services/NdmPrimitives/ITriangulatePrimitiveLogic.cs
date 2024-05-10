using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public interface ITriangulatePrimitiveLogic : ILogic
    {
        IEnumerable<INdmPrimitive> Primitives { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        List<INdm> GetNdms();
    }
}