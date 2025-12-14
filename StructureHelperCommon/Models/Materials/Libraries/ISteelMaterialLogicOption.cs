using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    internal interface ISteelMaterialLogicOption : IMaterialLogicOptions
    {
        double MaxPlasticStrainRatio { get; set; }
    }
}
