using Newtonsoft.Json;
using StructureHelperCommon.Models.Analyses;
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

        public object Clone()
        {
            return this;
        }
    }
}
