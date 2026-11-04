using Newtonsoft.Json;
using StructureHelperCommon.Models.Materials;

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

        public object Clone()
        {
            return this;
        }
    }
}
