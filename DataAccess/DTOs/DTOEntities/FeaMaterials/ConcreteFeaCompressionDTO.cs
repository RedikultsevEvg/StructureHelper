using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ConcreteFeaCompressionDTO : IConcreteFeaCompression
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Strength")]
        public double Strength { get; set; }
        [JsonProperty("PeakStrain")]
        public double PeakStrain { get; set; }
        [JsonProperty("ElasticStressRatio")]
        public double ElasticStressRatio { get; set; }
        [JsonProperty("DescendingScaleFactor")]
        public double DescendingScaleFactor { get; set; } = 1.0;

        public ConcreteFeaCompressionDTO(Guid id)
        {
            Id = id;
        }
    }
}
