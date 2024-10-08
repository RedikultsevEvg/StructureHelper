using Newtonsoft.Json;
using StructureHelperCommon.Models.Analyses;
using StructureHelperLogic.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
