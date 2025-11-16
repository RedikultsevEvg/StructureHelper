using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class Point2DRangeDTO : IPoint2DRange
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("StartPoint")]
        public IPoint2D StartPoint { get; set; }
        [JsonProperty("EndPoint")]
        public IPoint2D EndPoint { get; set; }


        public Point2DRangeDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
