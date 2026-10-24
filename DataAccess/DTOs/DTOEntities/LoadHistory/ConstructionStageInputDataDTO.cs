using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class ConstructionStageInputDataDTO : IConstructionStageInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonIgnore]
        public IConstructionStage ConstructionStage { get; }
        [JsonProperty("SubStepNumber")]
        public int SubStepNumber { get; set; }
        [JsonProperty("Duration")]
        public double Duration { get; set; }
        [JsonProperty("ForceAction")]
        public IForceAction ForceAction { get; set; }
        [JsonProperty("Primitives")]
        public List<INdmPrimitive> Primitives { get; } = [];

        public ConstructionStageInputDataDTO(Guid id, IConstructionStage constructionStage)
        {
            Id = id;
            ConstructionStage = constructionStage;
        }

    }
}
