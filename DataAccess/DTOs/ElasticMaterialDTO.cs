using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ElasticMaterialDTO : IElasticMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Modulus")]
        public double Modulus { get; set; }
        [JsonProperty("CompressiveStrength")]
        public double CompressiveStrength { get; set; }
        [JsonProperty("TensileStrength")]
        public double TensileStrength { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; } = new();


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }
    }
}
