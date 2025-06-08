using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class StirrupByDensityDTO : IStirrupByDensity
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; } = string.Empty;
        [JsonProperty("StirrupDensity")]
        public double StirrupDensity { get; set; }
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; }

        public StirrupByDensityDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
