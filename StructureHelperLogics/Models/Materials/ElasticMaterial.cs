using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
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
        private IElasticMaterialLogic elasticMaterialLogic => new ElasticMaterialLogic();
        public double Modulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }

        public ElasticMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var material = elasticMaterialLogic.GetLoaderMaterial(this, limitState, calcTerm);
            return material;
        }

        public object Clone()
        {
            return new ElasticMaterial() { Modulus = Modulus, CompressiveStrength = CompressiveStrength, TensileStrength = TensileStrength };
        }
    }
}
