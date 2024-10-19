using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class RectangleShapeDTO : IRectangleShape
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Width")]
        public double Width { get; set; }
        [JsonProperty("Height")]
        public double Height { get; set; }

    }
}
