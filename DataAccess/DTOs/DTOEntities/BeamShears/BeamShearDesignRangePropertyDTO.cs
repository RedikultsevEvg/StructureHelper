using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
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

        public BeamShearDesignRangePropertyDTO(Guid id)
        {
            Id = id;
        }

        public IBeamShearDesignRangeProperty GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            BeamShearDesignRangePropertyDTO newItem = new(id);
            return newItem;
        }
    }
}
