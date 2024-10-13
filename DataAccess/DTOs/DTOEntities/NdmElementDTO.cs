using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class NdmElementDTO : INdmElement
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("HeadMaterial")]
        public IHeadMaterial? HeadMaterial { get; set; } = new HeadMaterial();
        [JsonProperty("Triangulate")]
        public bool Triangulate { get; set; }
        [JsonProperty("UsersPrestrain")]
        public IForceTuple UsersPrestrain { get; set; } = new ForceTupleDTO();
        [JsonProperty("AutoPrestrain")]
        public IForceTuple AutoPrestrain { get; set; } = new ForceTupleDTO();


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
