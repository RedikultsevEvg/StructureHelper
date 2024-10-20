using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CrackCalculatorInputDataDTO : ICrackCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonProperty("ForceActions")]
        public List<IForceAction> ForceActions { get; set; } = new();
        [JsonProperty("ForcePrimitives")]
        public List<INdmPrimitive> Primitives { get; set; } = new();
        [JsonProperty("UserCrackInputData")]
        public IUserCrackInputData UserCrackInputData { get; set; } = new UserCrackInputDataDTO();

    }
}
