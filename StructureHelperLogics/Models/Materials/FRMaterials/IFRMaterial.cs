using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public interface IFRMaterial : IElasticMaterial
    {
        double ULSConcreteStrength { get; set; }
        double SumThickness { get; set; }
        double GammaF2 { get; }

    }
}
