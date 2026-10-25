using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorDTO : IHistoryCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("InputData")]
        public IHistoryCalculatorInputData InputData { get; set; }
        [JsonIgnore]
        public bool ShowTraceData { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get; set; }


        public HistoryCalculatorDTO(Guid id)
        {
            Id = id;
            InputData = new HistoryCalculatorInputDataDTO(Guid.NewGuid(), this);
        }

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
