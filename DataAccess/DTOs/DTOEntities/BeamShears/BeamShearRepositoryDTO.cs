using Newtonsoft.Json;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearRepositoryDTO : IBeamShearRepository
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Actions")]
        public List<IBeamShearAction> Actions { get; } = new();
        [JsonProperty("Sections")]
        public List<IBeamShearSection> Sections { get; } = new();
        [JsonProperty("Stirrups")]
        public List<IStirrup> Stirrups { get; } = new();
        [JsonProperty("Calculators")]
        public List<ICalculator> Calculators { get; } = new();
        public BeamShearRepositoryDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
