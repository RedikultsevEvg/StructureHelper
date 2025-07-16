using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class StirrupByInclinedRebarDTO : IStirrupByInclinedRebar
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; }
        [JsonProperty("StartCoordinate")]
        public double StartCoordinate { get; set; }
        [JsonProperty("TransferLength")]
        public double TransferLength { get; set; }
        [JsonProperty("AngleOfInclination")]
        public double AngleOfInclination { get; set; }
        [JsonProperty("LegCount")]
        public double LegCount { get; set; }
        [JsonProperty("RebarSection")]
        public IRebarSection RebarSection { get; set; }


        public StirrupByInclinedRebarDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
