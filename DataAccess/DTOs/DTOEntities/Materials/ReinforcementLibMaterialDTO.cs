using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class ReinforcementLibMaterialDTO : IReinforcementLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Reinforcement;
        [JsonProperty("Id")]
        public Guid Id { get; }


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

        public ReinforcementLibMaterialDTO(Guid id)
        {
            Id = id;
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
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            ReinforcementLibMaterialDTO newItem = new(id);
            if (sourceObject is not IReinforcementLibMaterial reinforcement)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            new ReinforcementLibUpdateStrategy().Update(newItem, reinforcement);
            return newItem;
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ReinforcementLibMaterialDTO newItem = new(id);
            return newItem;
        }
    }
}
