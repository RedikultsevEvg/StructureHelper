using Newtonsoft.Json;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.BeamShears;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearCalculatorDTO : IBeamShearCalculator
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("InputData")]
        public IBeamShearCalculatorInputData InputData { get; set; } = new BeamShearCalculatorInputDataDTO(Guid.Empty);
        [JsonProperty("ShowTraceData")]
        public bool ShowTraceData { get; set; }
        [JsonIgnore]
        public IResult Result => throw new NotImplementedException();
        [JsonIgnore]
        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public BeamShearCalculatorDTO(Guid id)
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
    }
}
