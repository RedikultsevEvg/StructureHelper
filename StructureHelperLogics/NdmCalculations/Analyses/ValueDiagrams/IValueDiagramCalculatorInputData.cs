using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    /// <summary>
    /// Implements input data for Value diagram calculator
    /// </summary>
    public interface IValueDiagramCalculatorInputData : ISaveable, IInputData, IHasForcesAndPrimitives
    {
        IStateCalcTermPair StateTermPair { get; set; }
        /// <summary>
        /// Collection of diagram for calculation
        /// </summary>
        List<IValueDiagramEntity> Diagrams { get; }
        bool CheckStrainLimit { get; set; }
    }
}
