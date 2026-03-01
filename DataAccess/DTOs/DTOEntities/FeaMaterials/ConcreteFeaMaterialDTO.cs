using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ConcreteFeaMaterialDTO : IConcreteFeaMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("YoungsModulus")]
        public double YoungsModulus { get; set; }
        [JsonProperty("PoissonsRatio")]
        public double PoissonsRatio { get; set; }
        [JsonProperty("Compression")]
        public IConcreteFeaCompression CompressionProperties { get; set; }
        [JsonProperty("Tension")]
        public IConcreteFeaTension TensionProperties { get; set; }



        public ConcreteFeaMaterialDTO(Guid id)
        {
            Id = id;
        }
    }
}
