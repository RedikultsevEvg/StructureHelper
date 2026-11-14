using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

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

        public ForceCombinationListDTO()
            : this(Guid.NewGuid())
        {
        }

        public ForceCombinationListDTO(Guid id)
        {
            Id = id;
        }

        public IForceCombinationList GetCombination()
        {
            throw new NotImplementedException();
        }

        public List<IForceCombinationList> GetCombinations()
        {
            throw new NotImplementedException();
        }


        public IForceCombinationList GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ForceCombinationListDTO neItem = new(id);
            return neItem;
        }
    }
}
