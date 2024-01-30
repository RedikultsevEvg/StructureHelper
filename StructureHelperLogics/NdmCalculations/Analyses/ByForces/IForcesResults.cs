using StructureHelperCommon.Models.Calculators;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForcesResults : IResult
    {
        string Description { get; set; }
        List<IForcesTupleResult> ForcesResultList { get; }
        bool IsValid { get; set; }
    }
}