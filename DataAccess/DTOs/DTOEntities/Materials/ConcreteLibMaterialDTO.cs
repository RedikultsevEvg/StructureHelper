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

        public ConcreteLibMaterialDTO()
            : this(Guid.NewGuid())
        {
        }

        public ConcreteLibMaterialDTO(Guid id)
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
            ConcreteLibMaterialDTO newItem = new(id);
            if (sourceObject is not IConcreteLibMaterial concrete)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            new ConcreteLibUpdateStrategy().Update(newItem, concrete);
            return newItem;
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ConcreteLibMaterialDTO newItem = new(id);
            var updateStragy = new ConcreteLibUpdateStrategy();
            updateStragy.Update(newItem, this);
            return newItem;
        }
    }
}
