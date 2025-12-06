using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    /// <summary>
    /// Input data for calculator of curvature of cross-section
    /// </summary>
    public interface ICurvatureCalculatorInputData : IInputData, ISaveable, IHasForceActions, IHasPrimitives
    {
        IDeflectionFactor DeflectionFactor { get; set; }
        bool ConsiderSofteningFactor { get; set; }
    }
}
