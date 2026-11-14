using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
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
        public double YoungModulus { get; set; }
        [JsonProperty("PoissonRatio")]
        public double PoissonRatio { get; set; }


        public ElasticFeaMaterialDTO(Guid id)
        {
            Id = id;
        }

        public IElasticFeaMaterial GetClone(IElasticFeaMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
