using System;

namespace StructureHelperCommon.Models.Materials
{
    public class PrimitiveMaterial : IPrimitiveMaterial
    {
        public string Id { get; }
        public MaterialTypes MaterialType { get; set; }
        public string ClassName { get; set; }
        public double Strength { get; set; }

        public PrimitiveMaterial()
        {
            Id = Convert.ToString(Guid.NewGuid());
        }
    }
}
