using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs.DTOEntities
{
    public class StirrupByRebarDTO : IStirrupByRebar
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("LegCount")]
        public double LegCount { get; set; }
        [JsonProperty("Diameter")]
        public double Diameter { get; set; }
        [JsonProperty("Material")]
        public IReinforcementLibMaterial Material { get; set; }
        [JsonProperty("Spacing")]
        public double Spacing { get; set; }
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; }

        public StirrupByRebarDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
