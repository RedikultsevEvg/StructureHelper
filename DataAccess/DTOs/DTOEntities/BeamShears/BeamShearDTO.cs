using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearDTO : IBeamShear
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Repository")]
        public IBeamShearRepository Repository { get; set; } = new BeamShearRepositoryDTO(Guid.NewGuid());

        public BeamShearDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
