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
    public class VisualAnalysisDTO : IVisualAnalysis
    {
    
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Analysis")]
        public IAnalysis Analysis { get; set; }
        [JsonIgnore]
        public Action ActionToRun { get; set; }

        public object Clone()
        {
            return this;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
