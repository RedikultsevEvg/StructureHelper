using Newtonsoft.Json;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    internal class HistoryCalculatorInputDataDTO : IHistoryCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonIgnore]
        public IHistoryCalculator HistoryCalculator { get; }
        [JsonProperty("ConstructionStages")]
        public List<IConstructionStage> ConstructionStages { get; } = [];
        [JsonProperty("Accuracy")]
        public IAccuracy Accuracy { get; set; } = new AccuracyDTO(Guid.NewGuid());
        [JsonProperty("CheckStrainLimit")]
        public bool CheckStainLimit { get; set; } = true;

        public HistoryCalculatorInputDataDTO(Guid id, IHistoryCalculator historyCalculator)
        {
            Id = id;
            HistoryCalculator = historyCalculator;
        }
    }
}
