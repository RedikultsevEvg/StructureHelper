using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public interface IGetForceTupleByFactoredTupleLogic : IGetForceTupleLogic
    {
        IFactoredForceTuple FactoredForceTuple { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
    }
}