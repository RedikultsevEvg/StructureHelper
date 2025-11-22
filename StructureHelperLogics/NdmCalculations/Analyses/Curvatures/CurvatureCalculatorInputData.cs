using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorInputData : ICurvatureCalculatorInputData
    {
        public Guid Id { get; }
        public List<IForceAction> ForceActions { get; } = [];

        public List<INdmPrimitive> Primitives { get; } = [];
        public double DeflectionFactor { get; set; } = 0.1042;
        public double SpanLength { get; set; } = 6.0;

        public CurvatureCalculatorInputData(Guid id)
        {
            Id = id;
        }
    }
}
