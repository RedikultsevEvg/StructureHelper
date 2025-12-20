using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public interface ISteelLibMaterial : ILibMaterial
    {
        double UlsFactor { get; set; }
        double SlsFactor { get; set; }
        double WorkConditionFactor { get; set; }
        double ThicknessFactor { get; set; }
        double MaxPlasticStrainRatio { get; set; }
    }
}
