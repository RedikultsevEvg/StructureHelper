using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class VertexDTO : IVertex
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Point")]
        public IPoint2D Point { get; set; }

        public VertexDTO(Guid id)
        {
            Id = id;
        }
    }
}
