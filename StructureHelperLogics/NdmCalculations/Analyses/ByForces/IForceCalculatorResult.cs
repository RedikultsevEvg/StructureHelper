using StructureHelperCommon.Models.Calculators;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceCalculatorResult : IResult
    {
        bool IsValid { get; set; }
        string Description { get; set; }
        List<IExtendedForceTupleCalculatorResult> ForcesResultList { get; }
    }
}