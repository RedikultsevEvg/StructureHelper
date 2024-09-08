using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class NdmPrimitiveDTO : INdmElement
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Center")]
        public IPoint2D Center { get; set; }
        [JsonProperty("HeadMaterial")]
        public IHeadMaterial? HeadMaterial { get; set; }
        [JsonProperty("Triangulate")]
        public bool Triangulate { get; set; }
        [JsonProperty("UserPrestrain")]
        public StrainTuple UsersPrestrain { get; } = new StrainTuple();
        [JsonIgnore]
        public StrainTuple AutoPrestrain => throw new NotImplementedException();


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
