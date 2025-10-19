using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class CircleShapeDTO : ICircleShape
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Diameter")]
        public double Diameter { get; set; }

        public CircleShapeDTO(Guid id)
        {
            Id = id;
        }
    }
}
