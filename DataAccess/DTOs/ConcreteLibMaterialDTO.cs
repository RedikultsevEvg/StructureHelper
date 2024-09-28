using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ConcreteLibMaterialDTO : IConcreteLibMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("RelativeHumidity")]
        public double RelativeHumidity { get; set; }
        [JsonProperty("MinAge")]
        public double MinAge { get; set; }
        [JsonProperty("MaxAge")]
        public double MaxAge { get; set; }
        [JsonProperty("MaterialEntity")]
        public ILibMaterialEntity MaterialEntity { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; }
        [JsonProperty("TensionForULS")]
        public bool TensionForULS { get; set; }
        [JsonProperty("TensionForSLS")]
        public bool TensionForSLS { get; set; }


        public IMaterialLogic MaterialLogic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<IMaterialLogic> MaterialLogics => throw new NotImplementedException();

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

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }
    }
}
