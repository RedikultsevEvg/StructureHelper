using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class TrapezoidShapeDTO : ITrapezoidShape
    {
        [JsonProperty("id")]
        public Guid Id { get; }
        [JsonProperty("Height")]
        public double Height { get; set; }
        [JsonProperty("TopBase")]
        public double TopBase { get; set; }
        [JsonProperty("BottomBase")]
        public double BottomBase { get; set; }
        [JsonProperty("TopBaseOffset")]
        public double TopBaseOffset { get; set; } = 0;
            

        public TrapezoidShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
