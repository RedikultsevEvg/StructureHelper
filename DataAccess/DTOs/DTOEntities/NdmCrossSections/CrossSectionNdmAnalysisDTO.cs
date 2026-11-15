using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Services;
using StructureHelperLogic.Models.Analyses;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class CrossSectionNdmAnalysisDTO : ICrossSectionNdmAnalysis
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Tags")]
        public string Tags { get; set; }
        [JsonProperty("VersionProcessor")]
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessorDTO(Guid.NewGuid());
        [JsonProperty("Comment")]
        public string Comment { get; set; } = string.Empty;
        [JsonProperty("Color")]
        public Color Color { get; set; } = new();

        public CrossSectionNdmAnalysisDTO(Guid id)
        {
            Id = id;
        }

        public IAnalysis GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            CrossSectionNdmAnalysisDTO newItem = new(id);
            return newItem;
        }
    }
}
