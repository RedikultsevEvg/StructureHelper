using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    /// <summary>
    /// Implements input data for Value diagram calculator
    /// </summary>
    public interface IValueDiagramCalculatorInputData : ISaveable, IHasForceActions, IHasPrimitives
    {
        IStateCalcTermPair StateTermPair { get; set; }
        /// <summary>
        /// Collection of diagram for calculation
        /// </summary>
        List<IValueDigram> Digrams { get; }
        bool CheckStrainLimit { get; set; }
    }
}
