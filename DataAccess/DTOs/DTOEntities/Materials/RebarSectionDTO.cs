using Newtonsoft.Json;
using StructureHelperLogics.Models.Materials;

namespace DataAccess.DTOs
{
    public class RebarSectionDTO : IRebarSection
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Material")]
        public IReinforcementLibMaterial Material { get; set; }
        [JsonProperty("Diameter")]
        public double Diameter { get; set; }


        public RebarSectionDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
