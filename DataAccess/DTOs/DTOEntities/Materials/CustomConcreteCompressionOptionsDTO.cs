using Newtonsoft.Json;
using StructureHelperCommon.Models.Materials;

namespace DataAccess.DTOs
{
    public class CustomConcreteCompressionOptionsDTO : ICustomConcreteCompressionOptions
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("PeakStress")]
        public double PeakStress { get; set; }

        [JsonProperty("PeakStrain")]
        public double PeakStrain { get; set; }

        [JsonProperty("LimitStrain")]
        public double LimitStrain { get; set; }

        [JsonProperty("DescendingScaleFactor")]
        public double DescendingScaleFactor { get; set; }

        public CustomConcreteCompressionOptionsDTO()
            : this(Guid.NewGuid())
        {
        }

        public CustomConcreteCompressionOptionsDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
