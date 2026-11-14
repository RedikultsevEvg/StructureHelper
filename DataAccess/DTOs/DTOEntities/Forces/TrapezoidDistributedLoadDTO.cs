using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class TrapezoidDistributedLoadDTO : ITrapezoidDistributedLoad
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("StartLoadValue")]
        public IForceTuple StartLoadValue { get; set; }
        [JsonProperty("EndLoadValue")]
        public IForceTuple EndLoadValue { get; set; }
        [JsonProperty("StartCoordinate")]
        public double StartCoordinate { get; set; }
        [JsonProperty("EndCoordinate")]
        public double EndCoordinate { get; set; }
        [JsonProperty("RelativeLoadLevel")]
        public double RelativeLoadLevel { get; set; }
        [JsonProperty("LoadRatio")]
        public double LoadRatio { get; set; }
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationPropertyDTO(Guid.Empty);


        public TrapezoidDistributedLoadDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public ITrapezoidDistributedLoad GetClone(ITrapezoidDistributedLoad sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IBeamSpanLoad GetClone(IBeamSpanLoad sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
