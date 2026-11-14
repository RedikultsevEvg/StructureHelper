using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class BeamShearAnalysisDTO : IBeamShearAnalysis
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Tags")]
        public string Tags { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; } = string.Empty;
        [JsonProperty("Color")]
        public Color Color { get; set; }
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessorDTO(Guid.NewGuid());
        public BeamShearAnalysisDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IAnalysis GetClone(IAnalysis sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
