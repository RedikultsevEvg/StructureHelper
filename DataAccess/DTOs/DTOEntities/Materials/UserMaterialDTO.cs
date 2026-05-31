using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;

namespace DataAccess.DTOs
{
    public class UserMaterialDTO : IUserMaterial
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("FilePath")]
        public string? FilePath { get; set; }
        [JsonProperty("SafetyFactors")]
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];


        public UserMaterialDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            return this;
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
