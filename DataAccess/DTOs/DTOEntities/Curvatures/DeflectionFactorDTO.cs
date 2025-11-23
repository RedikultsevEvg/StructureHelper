using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace DataAccess.DTOs
{
    public class DeflectionFactorDTO : IDeflectionFactor
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("DeflectionFactors")]
        public IForceTuple DeflectionFactors { get; set; }
        [JsonProperty("SpanLength")]
        public double SpanLength { get; set; }
        [JsonProperty("MaxDeflections")]
        public IForceTuple MaxDeflections { get; set; }


        public DeflectionFactorDTO(Guid id)
        {
            Id = id;
        }
    }
}
