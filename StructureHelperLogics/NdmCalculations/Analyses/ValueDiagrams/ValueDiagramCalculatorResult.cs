using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorResult : IValueDiagramCalculatorResult
    {
        public IValueDiagramCalculatorInputData? InputData { get; set; }

        public List<IForceTupleCalculatorResult> ForceTupleResults { get; set; } = [];

        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public List<IValueDiagramEntityResult> EntityResults { get; set; } = [];
    }
}
