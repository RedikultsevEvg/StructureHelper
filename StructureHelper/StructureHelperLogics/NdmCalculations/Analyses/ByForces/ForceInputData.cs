using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceInputData : IForceInputData
    {
        public IEnumerable<IForceCombinationList> ForceCombinationLists { get; set; }
        public IEnumerable<LimitStates> LimitStates { get; set; }
        public IEnumerable<CalcTerms> CalcTerms { get; set; }
        public IIterationProperty IterationProperty { get; }
    }
}
