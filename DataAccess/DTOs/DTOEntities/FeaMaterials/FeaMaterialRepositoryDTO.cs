using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
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

        public IFeaMaterialRepository GetClone(IFeaMaterialRepository sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
