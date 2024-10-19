using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorDTO : IForceCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public IForceCalculatorInputData InputData { get; set; } = new ForceCalculatorInputDataDTO();
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }
        [JsonIgnore]
        public Action<IResult> ActionToOutputResults { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();

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
