using Newtonsoft.Json;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.States;

namespace DataAccess.DTOs
{
    public class CustomConcreteModelDTO : ICustomConcreteModel
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("YoungModulus")]
        public double YoungModulus { get; set; }

        [JsonProperty("StateCalcTermPair")]
        public IStateCalcTermPair StateCalcTermPair { get; set; } = new StateCalcTermPairDTO();

        [JsonProperty("CompressionOptions")]
        public ICustomConcreteCompressionOptions CompressionOptions { get; set; } =
            new CustomConcreteCompressionOptionsDTO();

        [JsonProperty("TensionOptions")]
        public ICustomConcreteTensionOptions TensionOptions { get; set; } =
            new CustomConcreteTensionOptionsDTO();

        public CustomConcreteModelDTO()
            : this(Guid.NewGuid())
        {
        }

        public CustomConcreteModelDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
