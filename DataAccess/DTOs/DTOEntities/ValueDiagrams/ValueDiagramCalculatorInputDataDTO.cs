using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ValueDiagramCalculatorInputDataDTO : IValueDiagramCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("StateTermPair")]
        public IStateCalcTermPair StateTermPair { get; set; } = new StateCalcTermPairDTO();
        [JsonProperty("Diagrams")]
        public List<IValueDiagramEntity> Diagrams { get; } = [];
        [JsonProperty("CheckStrainLimits")]
        public bool CheckStrainLimit { get; set; } = true;
        [JsonProperty("ForceActions")]
        public List<IForceAction> ForceActions { get; } = [];
        [JsonProperty("Primitives")]
        public List<INdmPrimitive> Primitives { get; } = [];


        public ValueDiagramCalculatorInputDataDTO(Guid id)
        {
            Id = id;
        }


    }
}
