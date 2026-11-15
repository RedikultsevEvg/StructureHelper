using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class StirrupGroupDTO : IStirrupGroup
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; } = string.Empty;
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; } = 0;
        [JsonProperty("Stirrups")]
        public List<IStirrup> Stirrups { get; } = new();
        [JsonProperty("VisualProperty")]
        public IPrimitiveVisualProperty VisualProperty { get; set; }
        [JsonProperty("CheckMinimumIntensity")]
        public bool CheckMinimumIntensity { get; set; }

        public StirrupGroupDTO(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualPropertyDTO(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Black")
            };
        }

        public IStirrup GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            StirrupGroupDTO newItem = new(id);
            return newItem;
        }
    }
}
