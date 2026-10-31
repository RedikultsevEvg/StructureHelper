using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.Infrastructures.LoadHistory
{
    /// <summary>
    /// Minimal implementation of <see cref="IStageResult"/> for unit tests of the load history logics.
    /// </summary>
    internal sealed class StageResultStub : IStageResult
    {
        public StageResultStub(IConstructionStage stage)
        {
            Stage = stage;
        }

        public IConstructionStage Stage { get; }

        public List<ISubStageResult> SubStagesResults { get; } = [];

        public Dictionary<INdmPrimitive, INdmPrimitive> Primitives { get; } = [];

        public bool IsValid { get; set; } = true;

        public string? Description { get; set; } = string.Empty;
    }
}
