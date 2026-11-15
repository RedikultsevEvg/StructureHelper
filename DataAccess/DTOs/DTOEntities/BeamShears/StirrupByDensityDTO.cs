using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class StirrupByDensityDTO : IStirrupByDensity
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; } = string.Empty;
        [JsonProperty("StirrupDensity")]
        public double StirrupDensity { get; set; } = 30000;
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; } = 0;
        [JsonProperty("StartCoordinate")]
        public double StartCoordinate { get; set; } = 0;
        [JsonProperty("EndCoordinate")]
        public double EndCoordinate { get; set; } = 100;
        [JsonProperty("VisualProperty")]
        public IPrimitiveVisualProperty VisualProperty { get; set; }
        [JsonProperty("CheckMinimumIntensity")]
        public bool CheckMinimumIntensity { get; set; } = true;

        public StirrupByDensityDTO(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualPropertyDTO(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Gray"),
                Opacity = 0.5
            };
        }

        public IStirrup GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            StirrupByDensityDTO newItem = new(id);
            return newItem;
        }
    }
}
