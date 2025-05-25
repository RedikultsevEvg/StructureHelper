using Newtonsoft.Json;
using StructureHelperCommon.Models.Calculators;

namespace DataAccess.DTOs
{
    public class AccuracyDTO : IAccuracy
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("IterationAccuracy")]
        public double IterationAccuracy { get; set; }
        [JsonProperty("MaxIterationCount")]
        public int MaxIterationCount { get; set; }
        public AccuracyDTO(Guid id)
        {
            Id = id;
        }
    }
}
