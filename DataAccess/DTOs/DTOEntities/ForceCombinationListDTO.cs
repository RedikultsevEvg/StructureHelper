using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCombinationListDTO : IForceCombinationList
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("SetInGravityCenter")]
        public bool SetInGravityCenter { get; set; }
        [JsonProperty("ForcePoint")]
        public IPoint2D ForcePoint { get; set; } = new Point2DDTO();
        [JsonProperty("DesignForces")]
        public List<IDesignForceTuple> DesignForces { get; set; } = new();

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IForceCombinationList GetCombinations()
        {
            throw new NotImplementedException();
        }
    }
}
