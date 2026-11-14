using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class SteelLibMaterialDTO : ISteelLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Steel;
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("UlsFactor")]
        public double UlsFactor { get; set; } = 1.025;
        [JsonProperty("SlsFactor")]
        public double SlsFactor { get; set; } = 1.0;
        [JsonProperty("WorkConditionFactor")]
        public double WorkConditionFactor { get; set; } = 1.0;
        [JsonProperty("ThicknessFactor")]
        public double ThicknessFactor { get; set; } = 1.0;
        [JsonProperty("MaxPlasticStrainRatio")]
        public double MaxPlasticStrainRatio { get; set; } = 3.0;
        [JsonIgnore]
        public ILibMaterialEntity MaterialEntity { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];
        [JsonProperty("MaterialEntityId")]
        public Guid MaterialEntityId
        {
            get => MaterialEntity.Id;
            set
            {
                MaterialEntity = ProgramSetting.MaterialRepository.Repository.Single(x => x.Id == value);
            }
        }
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



        public SteelLibMaterialDTO(Guid id)
        {
            Id = id;
        }

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

        public IHelperMaterial GetClone(IHelperMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            SteelLibMaterialDTO newItem = new(id);
            return newItem;
        }
    }
}
