using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class FRMaterial : IFRMaterial
    {
        private IElasticMaterialLogic elasticMaterialLogic => new ElasticMaterialLogic();
        private MaterialTypes materialType;
        public double Modulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }

        public List<IMaterialSafetyFactor> SafetyFactors { get; }

        public FRMaterial(MaterialTypes materialType)
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            this.materialType = materialType;
            SafetyFactors.AddRange(PartialCoefficientFactory.GetDefaultFRSafetyFactors(ProgramSetting.FRCodeType, this.materialType));
        }

        public object Clone()
        {
            var newItem = new FRMaterial(materialType)
            { 
                Modulus = Modulus,
                CompressiveStrength = CompressiveStrength,
                TensileStrength = TensileStrength
            };
            return newItem;
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var material = elasticMaterialLogic.GetLoaderMaterial(this, limitState, calcTerm);
            return material;
        }
    }
}
