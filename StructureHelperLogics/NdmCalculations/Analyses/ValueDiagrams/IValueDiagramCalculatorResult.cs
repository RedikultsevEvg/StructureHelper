using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramCalculatorResult : IResult
    {
        IValueDiagramCalculatorInputData? InputData { get; set; }
        List<IValueDiagramEntityResult> EntityResults { get; set; }
        List<IForceTupleCalculatorResult> ForceTupleResults { get; set; }
    }
}
