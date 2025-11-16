using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
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

        public object Clone()
        {
            return this;
        }
    }
}
