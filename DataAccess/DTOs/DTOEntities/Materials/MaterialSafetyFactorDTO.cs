using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class MaterialSafetyFactorDTO : IMaterialSafetyFactor
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("Take")]
        public bool Take { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty("PartialFactors")]
        public List<IMaterialPartialFactor> PartialFactors { get; } = new();


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public double GetFactor(StressStates stressState, CalcTerms calcTerm, LimitStates limitStates)
        {
            throw new NotImplementedException();
        }
    }
}
