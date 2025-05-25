using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs.DTOEntities
{
    public class ConcentratedForceDTO : IConcentratedForce
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("ForceValue")]
        public IForceTuple ForceValue { get; set; }
        [JsonProperty("ForceCoordinate")]
        public double ForceCoordinate { get; set; }
        [JsonProperty("RelativeLoadLevel")]
        public double RelativeLoadLevel { get; set; }
        [JsonProperty("LoadRatio")]
        public double LoadRatio { get; set; }
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
