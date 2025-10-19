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


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
