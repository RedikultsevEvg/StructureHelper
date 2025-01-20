using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrackCalculatorDTO : ICrackCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public ICrackCalculatorInputData InputData { get; set; }
        [JsonIgnore]
        public IResult Result { get; }
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackCalculatorDTO(Guid id)
        {
            Id = id;
        }

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
