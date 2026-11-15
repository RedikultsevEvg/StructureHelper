using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class NdmElementDTO : INdmElement
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("HeadMaterial")]
        public IHeadMaterial? HeadMaterial { get; set; } = new HeadMaterial();
        [JsonProperty("Triangulate")]
        public bool Triangulate { get; set; }
        [JsonProperty("UsersPrestrain")]
        public IForceTuple UsersPrestrain { get; set; } = new ForceTupleDTO(Guid.NewGuid());
        [JsonIgnore]
        public IForceTuple AutoPrestrain { get; set; } = new ForceTupleDTO(Guid.NewGuid());
        [JsonIgnore]
        public IForceTuple HistoryPrestrain { get; set; } = new ForceTupleDTO(Guid.NewGuid());

        public NdmElementDTO(Guid id)
        {
            Id = id;
        }

        public INdmElement GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            NdmElementDTO newItem = new(id);
            return newItem;
        }
    }
}
