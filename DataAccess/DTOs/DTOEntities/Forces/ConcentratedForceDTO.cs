using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ConcentratedForceDTO : IConcentratedForce
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("ForceValue")]
        public IForceTuple ForceValue { get; set; } = new ForceTupleDTO(Guid.Empty);
        [JsonProperty("ForceCoordinate")]
        public double ForceCoordinate { get; set; }
        [JsonProperty("RelativeLoadLevel")]
        public double RelativeLoadLevel { get; set; }
        [JsonProperty("LoadRatio")]
        public double LoadRatio { get; set; }
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationPropertyDTO(Guid.Empty);

        public ConcentratedForceDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public IConcentratedForce GetClone(IConcentratedForce sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IBeamSpanLoad GetClone(IBeamSpanLoad sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
