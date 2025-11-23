using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorInputData : IInputData, IHasPrimitives
    {
        IForceTuple LongTermTuple { get; set; }
        IForceTuple ShortTermTuple { get; set; }
        IDeflectionFactor DeflectionFactor { get; set; }
    }
}
