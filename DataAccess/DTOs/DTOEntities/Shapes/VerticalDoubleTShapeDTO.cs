using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class VerticalDoubleTShapeDTO : IVerticalDoubleTShape
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("FullHeight")]
        public double FullHeight { get; set; }
        [JsonProperty("WebThickness")]
        public double WebThickness { get; set; }
        [JsonProperty("TopFlangeWidth")]
        public double TopFlangeWidth { get; set; }
        [JsonProperty("TopFlangeThickness")]
        public double TopFlangeThickness { get; set; }
        [JsonProperty("BottomFlangeWidth")]
        public double BottomFlangeWidth { get; set; }
        [JsonProperty("BottomFlangeThickness")]
        public double BottomFlangeThickness { get; set; }


        public VerticalDoubleTShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
