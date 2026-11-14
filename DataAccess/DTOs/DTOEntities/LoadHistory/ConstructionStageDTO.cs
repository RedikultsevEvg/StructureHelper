using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.LoadHistory;

namespace DataAccess.DTOs
{
    internal class ConstructionStageDTO : IConstructionStage
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public IConstructionStageInputData InputData { get; internal set; }

        public ConstructionStageDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            return this;
        }

        public IConstructionStage GetClone(IConstructionStage sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
