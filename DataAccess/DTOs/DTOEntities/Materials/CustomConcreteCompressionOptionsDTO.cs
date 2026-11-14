using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Services;

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

        public ICustomConcreteCompressionOptions GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CustomConcreteCompressionOptionsDTO newItem = new(id);
            new CustomConcreteCompressionOptionsUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
