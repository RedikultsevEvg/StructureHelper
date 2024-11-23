using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using StructureHelperLogics.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataAccess.DTOs
{
    public class CrossSectionNdmAnalysisDTO : ICrossSectionNdmAnalysis
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Tags")]
        public string Tags { get; set; }
        [JsonProperty("VersionProcessor")]
        public IVersionProcessor VersionProcessor { get; set; } = new VersionProcessorDTO();
        [JsonProperty("Comment")]
        public string Comment { get; set; } = string.Empty;
        [JsonProperty("Color")]
        public Color Color { get; set; } = new();

        public object Clone()
        {
            return this;
        }
    }
}
