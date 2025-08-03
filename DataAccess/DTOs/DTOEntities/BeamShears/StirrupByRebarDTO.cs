using Newtonsoft.Json;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class StirrupByRebarDTO : IStirrupByRebar
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; } = string.Empty;
        [JsonProperty("LegCount")]
        public double LegCount { get; set; } = 2;
        [JsonProperty("Diameter")]
        public double Diameter { get; set; } = 0.008;
        [JsonProperty("Material")]
        public IReinforcementLibMaterial Material { get; set; }
        [JsonProperty("Spacing")]
        public double Spacing { get; set; } = 0.1;
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; } = 0;
        [JsonProperty("IsSpiral")]
        public bool IsSpiral { get; set; } = false;
        [JsonProperty("StartCoordinate")]
        public double StartCoordinate { get; set; } = 0;
        [JsonProperty("EndCoordinate")]
        public double EndCoordinate { get; set; } = 100;
        [JsonProperty("VisualProperty")]
        public IPrimitiveVisualProperty VisualProperty { get; set; }

        public StirrupByRebarDTO(Guid id)
        {
            Id = id;
            VisualProperty = new PrimitiveVisualPropertyDTO(Guid.NewGuid())
            {
                Color = (Color)ColorConverter.ConvertFromString("Black")
            };
        }

        public object Clone()
        {
            return this;
        }
    }
}
