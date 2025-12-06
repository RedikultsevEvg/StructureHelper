using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureForceCalculatorInputData : ICurvatureForceCalculatorInputData
    {
        public IDesignForcePair ForcePair { get; set; }
        public List<INdmPrimitive> Primitives { get; set; } = [];
        public IDeflectionFactor DeflectionFactor { get; set; }
        public bool ConsiderSofteningFactor { get; set; }
    }
}
