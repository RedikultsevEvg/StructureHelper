using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class CustomConcreteTensionOptionsDTO : ICustomConcreteTensionOptions
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Strength")]
        public double Strength { get; set; }

        [JsonProperty("UltimateStrain")]
        public double UltimateStrain { get; set; }

        [JsonProperty("LimitStrain")]
        public double LimitStrain { get; set; }

        public CustomConcreteTensionOptionsDTO()
            : this(Guid.NewGuid())
        {
        }

        public CustomConcreteTensionOptionsDTO(Guid id)
        {
            Id = id;
        }

        public ICustomConcreteTensionOptions GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CustomConcreteTensionOptionsDTO newItem = new(id);
            new CustomConcreteTensionOptionsUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
