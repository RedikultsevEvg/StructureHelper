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
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ReinforcementLibMaterialDTO : IReinforcementLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Reinforcement;
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("MaterialEntityId")]
        public Guid MaterialEntityId
        {
            get => MaterialEntity.Id;
            set
            {
                MaterialEntity = ProgramSetting.MaterialRepository.Repository.Single(x => x.Id == value);
            }
        }
        [JsonIgnore]
        public ILibMaterialEntity MaterialEntity { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = new();
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
