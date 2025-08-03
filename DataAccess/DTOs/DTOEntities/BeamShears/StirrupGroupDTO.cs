using Newtonsoft.Json;
using StructureHelperCommon.Models.VisualProperties;
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

        public StirrupGroupDTO(Guid id)
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
