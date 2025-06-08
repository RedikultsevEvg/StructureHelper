using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class FactoredForceTupleDTO : IFactoredForceTuple
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("ForceTuple")]
        public IForceTuple ForceTuple { get; set; }
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; }
        
        public FactoredForceTupleDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
