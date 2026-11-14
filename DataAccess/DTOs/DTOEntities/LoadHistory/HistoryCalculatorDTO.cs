using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.Models.Materials;
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
            InputData = new HistoryCalculatorInputDataDTO(Guid.NewGuid());
        }

        public object Clone()
        {
            return this;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public ILogicCalculator GetClone(ILogicCalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            HistoryCalculatorDTO newItem = new(id);
            if (sourceObject is not IHistoryCalculator history)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            new HistoryCalculatorUpdateStrategy().Update(newItem, history);
            return newItem;
        }

        public ILogicCalculator GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            Guid id = GuidService.GetGuid(this, cloneMode);
            HistoryCalculatorDTO newItem = new(id);
            new HistoryCalculatorUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
