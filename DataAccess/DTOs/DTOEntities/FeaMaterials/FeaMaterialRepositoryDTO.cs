using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;

namespace DataAccess.DTOs
{
    public class FeaMaterialRepositoryDTO : IFeaMaterialRepository
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("FeaMaterials")]
        public List<IFeaMaterial> FeaMaterials { get; } = [];


        public FeaMaterialRepositoryDTO(Guid id)
        {
            Id = id;
        }
    }
}
