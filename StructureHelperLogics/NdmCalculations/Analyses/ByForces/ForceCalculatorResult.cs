namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculatorResult : IForceCalculatorResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public List<IExtendedForceTupleCalculatorResult> ForcesResultList { get; } = [];
    }
}
