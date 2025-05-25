using Newtonsoft.Json;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs.DTOEntities
{
    public class BeamShearDTO : IBeamShear
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Repository")]
        public IBeamShearRepository Repository { get; set; }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
