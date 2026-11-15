using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.FeaMaterials;
using StructureHelperCommon.Services;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class FeaMaterialAnalysisDTO : IFeaMaterialAnalysis
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Tags")]
        public string Tags { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; }
        [JsonProperty("Color")]
        public Color Color { get; set; }
        [JsonProperty("VersionProcessor")]
        public IVersionProcessor VersionProcessor { get; set; }


        public FeaMaterialAnalysisDTO(Guid id)
        {
            Id = id;
        }

        public IAnalysis GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            FeaMaterialAnalysisDTO newitem = new(id);
            return newitem;
        }
    }
}
