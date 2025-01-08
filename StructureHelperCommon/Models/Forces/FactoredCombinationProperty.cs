using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class FactoredCombinationProperty : IFactoredCombinationProperty
    {
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm;
        public LimitStates LimitState { get; set; } = LimitStates.SLS;
        public double LongTermFactor { get; set; } = 1d;
        public double ULSFactor { get; set; } = 1.2d;
    }
}
