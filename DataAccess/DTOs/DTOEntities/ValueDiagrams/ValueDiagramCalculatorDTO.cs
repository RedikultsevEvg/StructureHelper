using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
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

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ValueDiagramCalculatorDTO newItem = new (id);
            var updateStrategy = new ValueDiagramCalculatorUpdateStrategy() { UpdateChildren = false };
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
