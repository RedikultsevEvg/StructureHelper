using Newtonsoft.Json;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class UserCrackInputDataDTO : IUserCrackInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [JsonProperty("LengthBetweenCracks")]
        public double LengthBetweenCracks { get; set; }
        [JsonProperty("SetLengthBetweenCracks")]
        public bool SetLengthBetweenCracks { get; set; }
        [JsonProperty("SetSofteningFactors")]
        public bool SetSofteningFactor { get; set; }
        [JsonProperty("SofteningFactors")]
        public double SofteningFactor { get; set; }
        [JsonProperty("UltimateLongCrackWidths")]
        public double UltimateLongCrackWidth { get; set; }
        [JsonProperty("UltimateShortCrackWidths")]
        public double UltimateShortCrackWidth { get; set; }

    }
}
