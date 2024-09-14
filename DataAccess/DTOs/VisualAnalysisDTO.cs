using Newtonsoft.Json;
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

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
