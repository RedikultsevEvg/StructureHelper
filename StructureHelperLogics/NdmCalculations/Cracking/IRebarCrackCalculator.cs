using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IRebarCrackCalculator : ICalculator
    {
        Action<IResult> ActionToOutputResults { get; set; }
        RebarCrackCalculatorInputData InputData { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}