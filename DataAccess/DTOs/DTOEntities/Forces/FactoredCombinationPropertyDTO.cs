using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class FactoredCombinationPropertyDTO : IFactoredCombinationProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("CalctTerm")]
        public CalcTerms CalcTerm { get; set; }
        [JsonProperty("LimitState")]
        public LimitStates LimitState { get; set; }
        [JsonProperty("LongTermFactor")]
        public double LongTermFactor { get; set; }
        [JsonProperty("ULSFactor")]
        public double ULSFactor { get; set; }
        public FactoredCombinationPropertyDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
