using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ProjectDTO : IProject
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonIgnore]
        public string FullFileName { get; set; }
        [JsonIgnore]
        public bool IsNewFile { get; set; }
        [JsonIgnore]
        public bool IsActual { get; set; }

        [JsonProperty("VisualAnalyses")]
        public List<IVisualAnalysis> VisualAnalyses { get; private set; } = new();

        [JsonIgnore]
        public string FileName { get; set; }
    }
}
