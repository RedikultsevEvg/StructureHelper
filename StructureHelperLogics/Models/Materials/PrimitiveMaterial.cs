using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using System;

namespace StructureHelperLogics.Models.Materials
{
    public class PrimitiveMaterial : IPrimitiveMaterial
    {
        public string Id { get; }
        public MaterialTypes MaterialType { get; set; }
        public CodeTypes CodeType { get; set; }
        IHeadMaterial HeadMaterial { get; set; }
        public string ClassName { get; set; }
        public double Strength { get; set; }


        public PrimitiveMaterial()
        {
            Id = Convert.ToString(Guid.NewGuid());
        }
    }
}
