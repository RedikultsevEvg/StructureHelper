using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class Point2DDTO : IPoint2D
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("X")]
        public double X { get; set; }
        [JsonProperty("Y")]
        public double Y { get; set; }


        public Point2DDTO()
            : this(Guid.NewGuid())
        {
        }

        public Point2DDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var newItem = new Point2DDTO(Id);
            new Point2DUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
