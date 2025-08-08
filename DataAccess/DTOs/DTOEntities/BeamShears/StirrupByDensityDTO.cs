using Newtonsoft.Json;
using StructureHelperCommon.Models.VisualProperties;
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

        public StirrupByDensityDTO(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualPropertyDTO(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Gray"),
                Opacity = 0.5
            };
        }

        public object Clone()
        {
            return this;
        }
    }
}
