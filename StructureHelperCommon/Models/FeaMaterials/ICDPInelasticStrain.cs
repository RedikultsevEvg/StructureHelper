using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface ICDPInelasticStrain
    {
        List<double> ElasticStrainList { get; }
        List<double> InelasticStrainList { get; }
        List<double> StressList { get; }
        List<double> DamageList { get; }
        List<double> PlasticStrainList { get; }
    }
}
