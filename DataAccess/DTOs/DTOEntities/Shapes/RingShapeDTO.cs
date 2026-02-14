using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class RingShapeDTO : IRingShape
    {
        [JsonProperty("id")]
        public Guid Id { get; }
        [JsonProperty("OuterDiameter")]
        public double OuterDiameter { get; set; }
        [JsonProperty("InnerDiameter")]
        public double InnerDiameter { get; set; }
        [JsonIgnore]
        public double OuterRadius { get; set; }
        [JsonIgnore]
        public double InnerRadius { get; set; }


        public RingShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
