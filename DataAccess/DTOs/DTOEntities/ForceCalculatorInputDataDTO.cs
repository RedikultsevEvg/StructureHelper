using DataAccess.DTOs.DTOEntities;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorInputDataDTO : IForceCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("ForceActions")]
        public List<IForceAction> ForceActions { get; set; } = new();
        [JsonProperty("Primitives")]
        public List<INdmPrimitive> Primitives { get; set; } = new();
        [JsonProperty("LimitStatesList")]
        public List<LimitStates> LimitStatesList { get; set; } = new();
        [JsonProperty("CalcTermList")]
        public List<CalcTerms> CalcTermsList { get; set; } = new();
        [JsonProperty("Accuracy")]
        public IAccuracy Accuracy { get; set; } = new AccuracyDTO();
        [JsonProperty("CompressedMember")]
        public ICompressedMember CompressedMember { get; set; } = new CompressedMemberDTO();
        [JsonIgnore]
        public List<IForceCombinationList> ForceCombinationLists { get; set; } = new();

    }
}
