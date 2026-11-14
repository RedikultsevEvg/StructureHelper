using LoaderCalculator.Data.Materials;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

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
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];


        public ElasticMaterialDTO()
            : this(Guid.NewGuid())
        {
        }

        public ElasticMaterialDTO(Guid id)
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

        public IHelperMaterial GetClone(IHelperMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ElasticMaterialDTO newItem = new(id);
            return newItem;
        }
    }
}
