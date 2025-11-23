using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorInputData : ICurvatureCalculatorInputData
    {
        public Guid Id { get; }
        public List<IForceAction> ForceActions { get; } = [];

        public List<INdmPrimitive> Primitives { get; } = [];
        public IDeflectionFactor DeflectionFactor { get; set; } = new DeflectionFactor(Guid.NewGuid());

        public CurvatureCalculatorInputData(Guid id)
        {
            Id = id;
        }
    }
}
