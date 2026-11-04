using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;

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

        public object Clone()
        {
            return this;
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }
    }
}
