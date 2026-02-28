using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IElasticFeaMaterial : IFeaMaterial
    {
        double YoungsModulus { get; set; }
        double PoissonsRatio { get; set; }
    }
}
