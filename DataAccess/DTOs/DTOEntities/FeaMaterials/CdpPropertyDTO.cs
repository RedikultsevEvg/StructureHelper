using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class CdpPropertyDTO : ICdpProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("DilationAngle")]
        public double DilationAngle { get; set; } = 35;
        [JsonProperty("Eccentricity")]
        public double Eccentricity { get; set; } = 0.001;
        [JsonProperty("Fb0Ratio")]
        public double Fb0Ratio { get; set; } = 1.16;
        [JsonProperty("KRatio")]
        public double KRatio { get; set; } = 0.667;
        [JsonProperty("Viscocity")]
        public double Viscosity { get; set; } = 0.0001;

        public CdpPropertyDTO(Guid id)
        {
            Id = id;
        }
    }
}
