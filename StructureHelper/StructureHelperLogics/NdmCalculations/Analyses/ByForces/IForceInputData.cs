using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceInputData
    {
        IEnumerable<CalcTerms> CalcTerms { get; set; }
        IEnumerable<IForceCombinationList> ForceCombinationLists { get; set; }
        IIterationProperty IterationProperty { get; }
        IEnumerable<LimitStates> LimitStates { get; set; }
    }
}