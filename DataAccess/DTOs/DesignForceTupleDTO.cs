using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class DesignForceTupleDTO : IDesignForceTuple
    {
        private IUpdateStrategy<IDesignForceTuple> updateStrategy;

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("LimitState")]
        public LimitStates LimitState { get; set; }
        [JsonProperty("CalcTerm")]
        public CalcTerms CalcTerm { get; set; }
        [JsonProperty("ForceTuple")]
        public IForceTuple ForceTuple { get; set; } = new ForceTupleDTO();


        public object Clone()
        {
            DesignForceTupleDTO newItem = new();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
