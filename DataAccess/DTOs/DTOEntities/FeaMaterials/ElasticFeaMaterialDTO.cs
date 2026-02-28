using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ElasticFeaMaterialDTO : IElasticFeaMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("YoungModulus")]
        public double YoungsModulus { get; set; }
        [JsonProperty("PoissonRatio")]
        public double PoissonsRatio { get; set; }


        public ElasticFeaMaterialDTO(Guid id)
        {
            Id = id;
        }
    }
}
