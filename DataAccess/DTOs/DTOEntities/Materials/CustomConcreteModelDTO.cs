using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Models.States;
using StructureHelperCommon.Services;

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

        public ICustomConcreteModel GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CustomConcreteModelDTO newModel = new(id);
            new CustomConcreteModelUpdateStrategy() { UpdateChildren = false, }.Update(newModel, this);
            return newModel;
        }
    }
}
