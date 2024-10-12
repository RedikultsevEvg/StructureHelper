using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public interface IElasticMaterial : IHelperMaterial
    {
        double Modulus { get; set; }
        double CompressiveStrength { get; set; }
        double TensileStrength { get; set; }
    }
}
