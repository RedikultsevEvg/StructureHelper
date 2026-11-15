using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorDTO : ICurvatureCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public ICurvatureCalculatorInputData InputData { get; set; }
        [JsonProperty("ShowTraceData")]
        public bool ShowTraceData { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }
        public CurvatureCalculatorDTO(Guid id)
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
            CurvatureCalculatorDTO newItem = new(id);
            return newItem;
        }
    }
}
