using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class CustomConcreteHelperMaterialDTO : ICustomConcreteHelperMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("MaterialModels")]
        public List<ICustomConcreteModel> MaterialModels { get; } = [];

        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];
        [JsonProperty("TensionForUlS")]
        public bool TensionForULS { get; set; }
        [JsonProperty("TensionForSLS")]
        public bool TensionForSLS { get; set; }

        public CustomConcreteHelperMaterialDTO()
            : this(Guid.NewGuid())
        {
        }

        public CustomConcreteHelperMaterialDTO(Guid id)
        {
            Id = id;
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
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
            CustomConcreteHelperMaterialDTO newItem = new(id);
            return newItem;
        }
    }
}
