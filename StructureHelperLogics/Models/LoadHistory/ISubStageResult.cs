using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface ISubStageResult : IResult
    {
        IConstructionStage ConstructionStage { get; }
        StateCalcTermPair StateCalcTermPair { get; set; }
        StagePosition StagePosition { get; set; }
        int LocalNumber { get; set; }
        int TotalNumber { get; set; }
        double LocalTime { get; set; }
        double TotalTime { get; set; }
        List<INdmPrimitive> Primitives { get; set; }
        IForceTuple ForceTuple { get; set; }
        IForceTuple StrainTuple { get; set; }
        IForceTupleCalculatorResult ForcesTupleResult { get; set; }
    }
}
