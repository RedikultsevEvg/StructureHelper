using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForcesResults : INdmResult
    {
        string Desctription { get; set; }
        List<IForcesTupleResult> ForcesResultList { get; }
        bool IsValid { get; set; }
    }
}