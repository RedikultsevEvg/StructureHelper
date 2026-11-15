using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
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

        public void Run()
        {
            throw new NotImplementedException();
        }

        public ICalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            CrackCalculatorDTO newItem = new(id);
            new CalculatorUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
