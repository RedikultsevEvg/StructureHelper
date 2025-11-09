using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    /// <inheritdoc/>
    public class ValueDiagramCalculatorInputData : IValueDiagramCalculatorInputData
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public List<IValueDiagramEntity> Digrams { get; } = [];
        /// <inheritdoc/>
        public List<IForceAction> ForceActions { get; } = [];
        /// <inheritdoc/>
        public List<INdmPrimitive> Primitives { get; } = [];
        public IStateCalcTermPair StateTermPair { get; set; } = new StateCalcTermPair() { LimitState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm};
        public bool CheckStrainLimit { get; set; } = true;

        public ValueDiagramCalculatorInputData(Guid id)
        {
            Id = id;
        }

    }
}
