using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs.DTOEntities
{
    internal class BeamShearCalculatorInputDataDTO : IBeamShearCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Actions")]
        public List<IBeamShearAction> Actions { get; } = new();
        [JsonProperty("Sections")]
        public List<IBeamShearSection> Sections { get; } = new();
        [JsonProperty("Stirrups")]
        public List<IStirrup> Stirrups { get; } = new();
    }
}
