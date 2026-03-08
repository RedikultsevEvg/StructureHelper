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
        [JsonProperty("YoungModulus")]
        public double YoungModulus { get; set; }
        [JsonProperty("PoissonRatio")]
        public double PoissonRatio { get; set; }
        [JsonProperty("CdpProperty")]
        public ICdpProperty CdpProperty { get; set; }
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
