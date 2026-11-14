using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;

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
            var newItem = new FactoredCombinationPropertyDTO(Id);
            new FactoredCombinationPropertyUpdateStrategy().Update(newItem, this);
            return newItem;
        }

        public IFactoredCombinationProperty GetClone(IFactoredCombinationProperty sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
