using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
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


        public IConstructionStage GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ConstructionStageDTO newItem = new(id);
            return newItem;
        }
    }
}
