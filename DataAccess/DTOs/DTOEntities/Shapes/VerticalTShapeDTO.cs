using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VerticalTShapeDTO : IVerticalTShape
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("FulHeight")]
        public double FullHeight { get; set; }
        [JsonProperty("WebWidth")]
        public double WebWidth { get; set; }
        [JsonProperty("FlangeHeight")]
        public double FlangeHeight { get; set; }
        [JsonProperty("FlangeWidth")]
        public double FlangeWidth { get; set; }
        [JsonProperty("FlangeXOffset")]
        public double FlangeXOffset { get; set; }


        public VerticalTShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
