using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class ForceFactoredListDTO : IForceFactoredList
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ForceTuples")]
        public List<IForceTuple> ForceTuples { get; } = new();
        [JsonProperty("SetInGravityCenter")]
        public bool SetInGravityCenter { get; set; }
        [JsonProperty("ForcePoint")]
        public IPoint2D ForcePoint { get; set; } = new Point2DDTO();
        [JsonProperty("CombinationProperty")]
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationPropertyDTO(Guid.NewGuid());

        public ForceFactoredListDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var newItem = new ForceFactoredListDTO(Id);
            new ForceFactoredListUpdateStrategy().Update(newItem, this);
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

        public IForceFactoredList GetClone(IForceFactoredList sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IAction GetClone(IAction sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IForceFactoredList GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new ForceFactoredListDTO(id);
            new ForceFactoredListUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
