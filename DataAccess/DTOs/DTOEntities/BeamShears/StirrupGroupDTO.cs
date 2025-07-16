using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class StirrupGroupDTO : IStirrupGroup
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; }
        [JsonProperty("Stirrups")]
        public List<IStirrup> Stirrups { get; } = new();
        public StirrupGroupDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
