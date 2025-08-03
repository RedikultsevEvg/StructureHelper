using Newtonsoft.Json;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class VisualPropertyDTO : IVisualProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("IsVisible")]
        public bool IsVisible { get; set; }
        [JsonProperty("Color")]
        public Color Color { get; set; }
        [JsonProperty("SetMaterialColor")]
        public bool SetMaterialColor { get; set; }
        [JsonProperty("ZIndex")]
        public int ZIndex { get; set; }
        [JsonProperty("Opacity")]
        public double Opacity { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
