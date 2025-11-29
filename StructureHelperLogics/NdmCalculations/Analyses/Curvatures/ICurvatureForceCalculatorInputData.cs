using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorInputData : IInputData, IHasPrimitives
    {
        IDesignForcePair ForcePair {get;set;}
        IDeflectionFactor DeflectionFactor { get; set; }
    }
}
