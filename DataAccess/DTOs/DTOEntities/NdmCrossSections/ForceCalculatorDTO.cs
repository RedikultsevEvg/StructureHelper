using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace DataAccess.DTOs
{
    public class ForceCalculatorDTO : IForceCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ShowTraceData")]
        public bool ShowTraceData { get; set; } = false;
        [JsonProperty("InputData")]
        public IForceCalculatorInputData InputData { get; set; }
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }
        [JsonIgnore]
        public Action<IResult> ActionToOutputResults { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();


        public ForceCalculatorDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
