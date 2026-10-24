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
        [JsonProperty("InputData")]
        public IConstructionStageInputData InputData { get; internal set; }

        public ConstructionStageDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            return this;
        }
    }
}
