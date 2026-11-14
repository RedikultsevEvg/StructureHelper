using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Analyses;

namespace DataAccess.DTOs
{
    public class VisualAnalysisDTO : IVisualAnalysis
    {
    
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Analysis")]
        public IAnalysis Analysis { get; set; }
        [JsonIgnore]
        public Action ActionToRun { get; set; }

        public VisualAnalysisDTO(Guid id)
        {
            Id = id;
        }
        public object Clone()
        {
            return this;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public IVisualAnalysis GetClone(IVisualAnalysis sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
