using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarCrackCalculator : ILogicCalculator
    {
        Action<IResult> ActionToOutputResults { get; set; }
        RebarCrackCalculatorInputData InputData { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}