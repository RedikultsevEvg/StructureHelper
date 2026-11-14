using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
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
        public Guid Id { get; }


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

        public CompressedMemberDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public ICompressedMember GetClone(ICompressedMember sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
