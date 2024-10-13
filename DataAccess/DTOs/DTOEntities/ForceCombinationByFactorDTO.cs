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
    public class ForceCombinationByFactorDTO : IForceCombinationByFactor
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("FullSLSForces")]
        public IForceTuple FullSLSForces { get; set; } = new ForceTupleDTO();
        [JsonProperty("ULSFactor")]
        public double ULSFactor { get; set; }
        [JsonProperty("LongTermFactor")]
        public double LongTermFactor { get; set; }
        [JsonProperty("SetInGravityCenter")]
        public bool SetInGravityCenter { get; set; }
        [JsonProperty("ForcePoint")]
        public IPoint2D ForcePoint { get; set; } = new Point2DDTO();


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
