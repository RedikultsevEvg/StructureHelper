using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackWidthCalculationLogic : ILogic
    {
        IRebarCrackCalculatorInputData InputData { get; set; }
        RebarCrackResult Result { get; }

        void Run();
        CrackWidthRebarTupleResult ProcessLongTermCalculations();
        CrackWidthRebarTupleResult ProcessShortTermCalculations();
        RebarStressResult GetRebarStressResult(IRebarCrackInputData inputData);
    }
}