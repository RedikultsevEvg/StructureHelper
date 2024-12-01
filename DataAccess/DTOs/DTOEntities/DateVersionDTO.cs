using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DateVersionDTO : IDateVersion
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("DateTime")]
        public DateTime DateTime { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; } = string.Empty;
        [JsonProperty("AnalysisVersion")]
        public ISaveable AnalysisVersion { get; set; }
    }
}
