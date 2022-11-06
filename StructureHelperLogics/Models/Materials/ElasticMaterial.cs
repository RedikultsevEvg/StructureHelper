using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class ElasticMaterial : IElasticMaterial
    {
        public double Modulus { get; set; }

        public object Clone()
        {
            return new ElasticMaterial() { Modulus = Modulus };
        }

        public IPrimitiveMaterial GetPrimitiveMaterial()
        {
            throw new NotImplementedException();
        }
    }
}
