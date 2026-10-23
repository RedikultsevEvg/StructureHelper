using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class StageResult : IStageResult
    {
        public IConstructionStage Stage { get; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;

        public List<ISubStageResult> SubStagesResults { get; } = [];

        public Dictionary<INdmPrimitive, INdmPrimitive> Primitives { get; } = [];

        public StageResult(IConstructionStage stage)
        {
            Stage = stage;
        }
    }
}
