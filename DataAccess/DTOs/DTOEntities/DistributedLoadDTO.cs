using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class DistributedLoadDTO : IDistributedLoad
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("LoadValue")]
        public IForceTuple LoadValue { get; set; }
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
        public DistributedLoadDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
