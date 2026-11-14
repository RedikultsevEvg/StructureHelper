using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class ForceCombinationFromFileDTO : IForceCombinationFromFile
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ForceFiles")]
        public List<IColumnedFileProperty> ForceFiles { get; set; } = new();
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationPropertyDTO(Guid.NewGuid());
        [JsonProperty("SetInGravityCenter")]
        public bool SetInGravityCenter { get; set; }
        [JsonProperty("ForcePoint")]
        public IPoint2D ForcePoint { get; set; } = new Point2DDTO();
        [JsonProperty("SkipWrongRows")]
        public bool SkipWrongRows { get; set; } = false;

        public ForceCombinationFromFileDTO(Guid id)
        {
            Id = id;
        }

        public List<IForceCombinationList> GetCombinations()
        {
            throw new NotImplementedException();
        }

        public IForceCombinationFromFile GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ForceCombinationFromFileDTO newItem = new(id);
            return newItem;
        }
    }
}
