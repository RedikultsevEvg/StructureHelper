using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class ConstructionStageDTO : IConstructionStage
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("SubStepNumber")]
        public int SubStepNumber { get; set; }
        [JsonProperty("Duration")]
        public double Duration { get; set; }
        [JsonProperty("ForceAction")]
        public IForceAction ForceAction { get; set; }
        [JsonProperty("Primitives")]
        public List<INdmPrimitive> Primitives { get; } = [];


        public ConstructionStageDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
