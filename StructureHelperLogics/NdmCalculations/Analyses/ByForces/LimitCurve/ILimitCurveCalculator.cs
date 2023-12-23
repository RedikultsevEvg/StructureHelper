using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurveCalculator : ICalculator, IHasActionByResult
    {
        Action<IResult> ActionToOutputResults { get; set; }
        SurroundData SurroundData { get; set; }
        int PointCount { get; set; }
        ISurroundProc SurroundProcLogic { get; set; }
    }
}