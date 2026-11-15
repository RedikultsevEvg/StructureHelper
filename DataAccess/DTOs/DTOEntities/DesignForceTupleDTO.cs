using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class DesignForceTupleDTO : IDesignForceTuple
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("LimitState")]
        public LimitStates LimitState { get; set; }
        [JsonProperty("CalcTerm")]
        public CalcTerms CalcTerm { get; set; }
        [JsonProperty("ForceTuple")]
        public IForceTuple ForceTuple { get; set; } = new ForceTupleDTO(Guid.NewGuid());

        public DesignForceTupleDTO()
            : this(Guid.NewGuid())
        {
        }

        public DesignForceTupleDTO(Guid id)
        {
            Id = id;
        }


        public IDesignForceTuple GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new DesignForceTupleDTO(id);
            new DesignForceTupleUpdateStrategy().Update(newItem, this);
            return newItem;

        }
    }
}
