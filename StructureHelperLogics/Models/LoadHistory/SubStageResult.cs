using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.Models.LoadHistory.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class SubStageResult : ISubStageResult
    {
        public IConstructionStage ConstructionStage { get; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public StagePosition StagePosition { get; set; }
        public int LocalNumber { get; set; }
        public int TotalNumber { get; set; }
        public double LocalTime { get; set; }
        public double TotalTime { get; set; }
        public IForceTuple ForceTuple { get; set; }
        public IForceTuple StrainTuple { get; set; } = new ForceTuple();
        public List<INdmPrimitive> Primitives { get; set; }
        public IForceTupleCalculatorResult ForcesTupleResult { get; set; }
        public StateCalcTermPair StateCalcTermPair { get; set; } = new();

        public SubStageResult(IConstructionStage constructionStage)
        {
            ConstructionStage = constructionStage;
        }
    }
}
