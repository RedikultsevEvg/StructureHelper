using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface ISteelMaterialLogicOption : IMaterialLogicOptions
    {
        double MaxPlasticStrainRatio { get; set; }
        double UlsFactor { get; set; }
        double SlsFactor { get; set; }
        double ThicknessFactor { get; set; }
        double WorkConditionFactor { get; set; }
    }
}
