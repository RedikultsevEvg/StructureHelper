using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramCalculatorDTO : IValueDiagramCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public IValueDiagramCalculatorInputData InputData { get; set; }
        [JsonProperty("ShowTraceData")]
        public bool ShowTraceData { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }


        public ValueDiagramCalculatorDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
