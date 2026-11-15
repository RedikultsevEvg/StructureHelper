using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
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

        public IRebarSection GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            RebarSectionDTO newItem = new (id);
            var logic = new RebarSectionUpdateStrategy();
            logic.Update(newItem, this);
            return newItem;
        }
    }
}
