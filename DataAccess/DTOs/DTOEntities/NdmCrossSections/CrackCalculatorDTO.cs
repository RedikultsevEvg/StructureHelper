using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.Logics;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace DataAccess.DTOs
{
    public class CrackCalculatorDTO : ICrackCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ShowTraceData")]
        public bool ShowTraceData { get; set; } = false;
        [JsonProperty("InputData")]
        public ICrackCalculatorInputData InputData { get; set; }
        [JsonIgnore]
        public IResult Result { get; }
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackCalculatorDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            CrackCalculatorDTO newItem = new(Id);
            new CalculatorUpdateStrategy().Update(newItem, this);
            return newItem;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
