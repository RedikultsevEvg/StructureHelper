using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public interface IValuePointDiagramLogic
    {
        ForceCalculator Calculator { get; set; }
        PointPrimitiveLogic PrimitiveLogic { get; set; }
        IEnumerable<IForcesTupleResult> TupleList { get; set; }
        ValueDelegatesLogic ValueDelegatesLogic { get; set; }

        GenericResult<ArrayParameter<double>> GetArrayParameter();
    }
}