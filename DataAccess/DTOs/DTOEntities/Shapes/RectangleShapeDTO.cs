using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class RectangleShapeDTO : IRectangleShape
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Width")]
        public double Width { get; set; }
        [JsonProperty("Height")]
        public double Height { get; set; }

        public RectangleShapeDTO(Guid id)
        {
            Id = id;
        }

    }
}
