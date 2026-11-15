using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Logics;

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

        public void Run()
        {
            throw new NotImplementedException();
        }

        public ILogicCalculator GetClone(ILogicCalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public ICalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            ForceCalculatorDTO newItem = new(id);
            new CalculatorUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
