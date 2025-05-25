using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs.DTOEntities
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


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
