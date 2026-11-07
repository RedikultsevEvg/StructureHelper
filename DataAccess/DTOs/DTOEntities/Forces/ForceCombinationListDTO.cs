using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;

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

        public object Clone()
        {
            var newItem = new ForceCombinationListDTO(Id);
            new ForceActionBaseUpdateStrategy().Update(newItem, this);
            new ForceCombinationListUpdateStrategy().Update(newItem, this);
            return newItem;
        }

        public IForceCombinationList GetCombination()
        {
            throw new NotImplementedException();
        }

        public List<IForceCombinationList> GetCombinations()
        {
            throw new NotImplementedException();
        }
    }
}
