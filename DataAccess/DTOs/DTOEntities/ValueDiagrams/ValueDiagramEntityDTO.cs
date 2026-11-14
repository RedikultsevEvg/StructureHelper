using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramEntityDTO : IValueDiagramEntity
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("IsTaken")]
        public bool IsTaken { get; set; }
        [JsonProperty("ValueDiagram")]
        public IValueDiagram ValueDiagram { get; set; }


        public ValueDiagramEntityDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public IValueDiagramEntity GetClone(IValueDiagramEntity sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
