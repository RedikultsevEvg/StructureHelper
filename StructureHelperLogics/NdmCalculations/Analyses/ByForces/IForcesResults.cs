using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForcesResults : INdmResult
    {
        string Desctription { get; set; }
        List<ForcesResult> ForcesResultList { get; }
        bool IsValid { get; set; }
    }
}