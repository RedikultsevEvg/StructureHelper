using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ILimitCurveCalculator : ILogicCalculator, IHasActionByResult
    {
        Action<IResult> ActionToOutputResults { get; set; }
        ISurroundData SurroundData { get; set; }
        int PointCount { get; set; }
        ISurroundProc SurroundProcLogic { get; set; }
    }
}