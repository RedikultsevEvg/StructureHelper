using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCombinationByFactorV1_0DTO : IForceFactoredList
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("LimitState")]
        public LimitStates LimitState { get; set; } = LimitStates.SLS;
        [JsonProperty("CalcTerm")]
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm;
        [JsonProperty("FullSLSForces")]
        public IForceTuple ForceTuple { get; set; } = new ForceTupleDTO(Guid.NewGuid());
        [JsonProperty("ULSFactor")]
        public double ULSFactor { get; set; }
        [JsonProperty("LongTermFactor")]
        public double LongTermFactor { get; set; }
        [JsonProperty("SetInGravityCenter")]
        public bool SetInGravityCenter { get; set; }
        [JsonProperty("ForcePoint")]
        public IPoint2D ForcePoint { get; set; } = new Point2DDTO();
        [JsonIgnore]
        public IFactoredCombinationProperty CombinationProperty
        {
            get
            {
                return new FactoredCombinationProperty(Guid.NewGuid())
                {
                    CalcTerm = CalcTerm,
                    LimitState = LimitState,
                    LongTermFactor = LongTermFactor,
                    ULSFactor = ULSFactor,
                };
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [JsonIgnore]
        public List<IForceTuple> ForceTuples => new() { ForceTuple};

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IForceCombinationList GetCombination()
        {
            throw new NotImplementedException();
        }

        public List<IForceCombinationList> GetCombinations()
        {
            throw new NotImplementedException();
        }
    }
}
