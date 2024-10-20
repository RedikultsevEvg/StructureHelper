using Newtonsoft.Json;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.DTOEntities
{
    public class AccuracyDTO : IAccuracy
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonProperty("IterationAccuracy")]
        public double IterationAccuracy { get; set; }
        [JsonProperty("MaxIterationCount")]
        public int MaxIterationCount { get; set; }

    }
}
