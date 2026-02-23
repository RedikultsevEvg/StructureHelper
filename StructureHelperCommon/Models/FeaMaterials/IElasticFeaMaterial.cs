using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IElasticFeaMaterial : IFeaMaterial
    {
        double YoungModulus { get; set; }
        double PoissonRatio { get; set; }
    }
}
