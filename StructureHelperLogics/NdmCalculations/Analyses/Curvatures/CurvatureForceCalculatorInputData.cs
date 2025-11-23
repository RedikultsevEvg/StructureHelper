using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureForceCalculatorInputData : ICurvatureForceCalculatorInputData
    {
        public IForceTuple LongTermTuple { get; set; }
        public IForceTuple ShortTermTuple { get; set; }
        public List<INdmPrimitive> Primitives { get; set; } = [];
        public IDeflectionFactor DeflectionFactor { get; set; }
    }
}
