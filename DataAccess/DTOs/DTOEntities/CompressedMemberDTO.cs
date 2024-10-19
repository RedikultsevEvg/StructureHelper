using Newtonsoft.Json;
using StructureHelperCommon.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CompressedMemberDTO : ICompressedMember
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Bucling")]
        public bool Buckling { get; set; }
        [JsonProperty("GeometryLength")]
        public double GeometryLength { get; set; }
        [JsonProperty("LengthFactorX")]
        public double LengthFactorX { get; set; }
        [JsonProperty("DiagramFactorX")]
        public double DiagramFactorX { get; set; }
        [JsonProperty("LengthFactorY")]
        public double LengthFactorY { get; set; }
        [JsonProperty("DiagramFactorY")]
        public double DiagramFactorY { get; set; }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
