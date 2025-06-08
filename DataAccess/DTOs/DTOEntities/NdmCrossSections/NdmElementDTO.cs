using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
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
        [JsonProperty("AutoPrestrain")]
        public IForceTuple AutoPrestrain { get; set; } = new ForceTupleDTO(Guid.NewGuid());

        public NdmElementDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
