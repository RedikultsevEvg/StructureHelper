using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearDesignRangePropertyDTO : IBeamShearDesignRangeProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("AbsoluteRangeValue")]
        public double AbsoluteRangeValue { get; set; } = 0.0;
        [JsonProperty("RelativeEffectiveDepthRangeValue")]
        public double RelativeEffectiveDepthRangeValue { get; set; } = 3.3;
        [JsonProperty("StepCount")]
        public int StepCount { get; set; } = 55;
        [JsonProperty("SectionLengthMaxValue")]
        public double RelativeEffectiveDepthSectionLengthMaxValue { get; set; } = 3;
        [JsonProperty("LengthMinValue")]
        public double RelativeEffectiveDepthSectionLengthMinValue { get; set; } = 0;

        public BeamShearDesignRangePropertyDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
