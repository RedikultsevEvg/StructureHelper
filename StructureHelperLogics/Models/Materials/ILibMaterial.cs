using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Infrastructures.CommonEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface ILibMaterial : IHelperMaterial
    {
        MaterialTypes MaterialType { get; set; }
        CodeTypes CodeType { get; set; }
        string Name { get; set; }
        double MainStrength { get; set; }
    }
}
