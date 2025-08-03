using Newtonsoft.Json;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services.ColorServices;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class PrimitiveVisualPropertyDTO : IPrimitiveVisualProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("IsVisible")]
        public bool IsVisible { get; set; } = true;
        [JsonProperty("Color")]
        public Color Color { get; set; } = ColorProcessor.GetRandomColor();
        [JsonProperty("Zindex")]
        public int ZIndex { get; set; } = 0;
        [JsonProperty("Opacity")]
        public double Opacity { get; set; } = 1;


        public PrimitiveVisualPropertyDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
