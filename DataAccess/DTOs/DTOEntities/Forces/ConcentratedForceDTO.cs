using Newtonsoft.Json;
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
    }
}
