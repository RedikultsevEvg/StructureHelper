using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
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

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return new ElasticMaterial() { Modulus = Modulus };
        }
    }
}
