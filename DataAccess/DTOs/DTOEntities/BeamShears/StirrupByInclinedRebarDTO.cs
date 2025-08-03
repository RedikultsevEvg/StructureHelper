using Newtonsoft.Json;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class StirrupByInclinedRebarDTO : IStirrupByInclinedRebar
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("CompressedGap")]
        public double CompressedGap { get; set; }
        [JsonProperty("StartCoordinate")]
        public double StartCoordinate { get; set; }
        [JsonProperty("TransferLength")]
        public double TransferLength { get; set; }
        [JsonProperty("AngleOfInclination")]
        public double AngleOfInclination { get; set; }
        [JsonProperty("LegCount")]
        public double LegCount { get; set; }
        [JsonProperty("RebarSection")]
        public IRebarSection RebarSection { get; set; }
        [JsonProperty("VisualProperty")]
        public IPrimitiveVisualProperty VisualProperty { get; set; }

        public StirrupByInclinedRebarDTO(Guid id)
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
