using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ConcreteLibMaterialDTO : IConcreteLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Concrete;


        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("RelativeHumidity")]
        public double RelativeHumidity { get; set; }
        [JsonProperty("MinAge")]
        public double MinAge { get; set; }
        [JsonProperty("MaxAge")]
        public double MaxAge { get; set; }
        [JsonProperty("MaterialEntityId")]
        public Guid MaterialEntityId
        {
            get
            {
                return MaterialEntity.Id;
            }

            set
            {
                MaterialEntity = ProgramSetting.MaterialRepository.Repository.Single(x => x.Id == value);
            }
        }
        [JsonIgnore]
        public ILibMaterialEntity MaterialEntity { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = new();
        [JsonProperty("TensionForULS")]
        public bool TensionForULS { get; set; }
        [JsonProperty("TensionForSLS")]
        public bool TensionForSLS { get; set; }
        [JsonProperty("MaterialLogicId")]
        public Guid MaterialLogicId
        {
            get => MaterialLogic.Id;
            set
            {
                MaterialLogic = MaterialLogics.Single(x => x.Id == value);
            }
        }
        [JsonIgnore]
        public IMaterialLogic MaterialLogic { get; set; }
        [JsonIgnore]
        public List<IMaterialLogic> MaterialLogics { get; } = ProgramSetting.MaterialLogics.Where(x => x.MaterialType == materialType).ToList();

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
