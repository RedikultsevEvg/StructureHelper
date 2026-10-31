using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.Infrastructures.LoadHistory
{
    /// <summary>
    /// Minimal implementation of <see cref="ISubStageResult"/> for unit tests of the load history logics.
    /// </summary>
    internal sealed class SubStageResultStub : ISubStageResult
    {
        public IConstructionStage ConstructionStage { get; set; } = null!;

        public StateCalcTermPair StateCalcTermPair { get; set; } = new();

        public StagePosition StagePosition { get; set; }

        public int LocalNumber { get; set; }

        public int TotalNumber { get; set; }

        public double LocalTime { get; set; }

        public double TotalTime { get; set; }

        public List<INdmPrimitive> Primitives { get; set; } = [];

        public IForceTuple ForceTuple { get; set; } = new ForceTuple();

        public IForceTuple StrainTuple { get; set; } = new ForceTuple();

        public IForceTupleCalculatorResult ForcesTupleResult { get; set; } = null!;

        public bool IsValid { get; set; } = true;

        public string? Description { get; set; } = string.Empty;
    }
}
