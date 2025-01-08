using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ForceFactoredCombinationPropertyDTO : IFactoredCombinationProperty
    {
        [JsonProperty("CalctTerm")]
        public CalcTerms CalcTerm { get; set; }
        [JsonProperty("LimitState")]
        public LimitStates LimitState { get; set; }
        [JsonProperty("LongTermFactor")]
        public double LongTermFactor { get; set; }
        [JsonProperty("ULSFactor")]
        public double ULSFactor { get; set; }
    }
}
