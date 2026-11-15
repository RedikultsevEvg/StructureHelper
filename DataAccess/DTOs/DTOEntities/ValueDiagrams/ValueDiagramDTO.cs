using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramDTO : IValueDiagram
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Point2DRange")]
        public IPoint2DRange Point2DRange { get; set; }
        [JsonProperty("StepNumber")]
        public int StepNumber { get; set; }

        public ValueDiagramDTO(Guid id)
        {
            Id = id;
        }

        public IValueDiagram GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ValueDiagramDTO newItem = new(id);
            return newItem;
        }
    }
}
