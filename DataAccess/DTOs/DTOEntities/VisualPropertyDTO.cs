using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;
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

        public VisualPropertyDTO()
            : this(Guid.NewGuid())
        {
        }

        public VisualPropertyDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IPrimitiveVisualProperty GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            VisualPropertyDTO newItem = new(id);
            return newItem;
        }
    }
}
