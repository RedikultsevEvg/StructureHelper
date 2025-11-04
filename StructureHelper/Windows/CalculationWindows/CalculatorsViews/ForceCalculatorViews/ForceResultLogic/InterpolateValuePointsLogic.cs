using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class InterpolateValuePointsLogic
    {
        private InterpolationProgressLogic interpolationLogic;
        private ValuePointsInterpolateViewModel viewModel;
        private IResult result;
        private ValuePointsInterpolationInputData inputData;
        public IExtendedForceTupleCalculatorResult SelectedResult { get; set; }
        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public IForceCalculator ForceCalculator { get; set; }


        public ILongProcessLogic ProgressLogic { get; set; }
        public ShowProgressLogic ShowProgressLogic { get; set; }

        public void InterpolateValuePoints()
        {
            var tuple = SelectedResult.ForcesTupleResult.ForceTuple ?? throw new StructureHelperException(ErrorStrings.NullReference + ": Design force combination");
            PrepareInputData(tuple);
            viewModel = new ValuePointsInterpolateViewModel(inputData);
            if (ShowDialog() == false) { return; };
            ShowDiagram(result);
        }

        private void PrepareInputData(IForceTuple endTuple)
        {
            inputData = new ValuePointsInterpolationInputData()
            {
                StartForceTuple = new ForceTuple(),
                FinishForceTuple = endTuple.Clone() as IForceTuple,
                StateCalcTermPair = SelectedResult.StateCalcTermPair,
            };
            inputData.PrimitiveBases.AddRange(PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(NdmPrimitives));
        }

        private bool ShowDialog()
        {
            var wnd = new ValuePointsInterpolateView(viewModel);
            wnd.ShowDialog();
            if (wnd.DialogResult != true) { return false; }
            interpolationLogic = new InterpolationProgressLogic(ForceCalculator, SelectedResult.StateCalcTermPair, viewModel.ForceInterpolationViewModel.Result);
            ProgressLogic = interpolationLogic;
            ShowProgressLogic = new(interpolationLogic)
            {
                WindowTitle = "Interpolate forces",
            };
            ShowProgressLogic.Show();
            result = interpolationLogic.InterpolateCalculator.Result;
            return true;
        }

        private void ShowDiagram(IResult result)
        {
            if (result.IsValid == false) { return; }
            if (result is not IForceCalculatorResult)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(IForceCalculatorResult), result));
            }
            var tupleResult = result as IForceCalculatorResult;
            var pointGraphLogic = new ShowValuePointDiagramLogic()
            {
                Calculator = interpolationLogic.InterpolateCalculator,
                PrimitiveLogic = viewModel.PrimitiveLogic,
                ValueDelegatesLogic = viewModel.ValueDelegatesLogic,
                TupleResultList = tupleResult.ForcesResultList
            };
            pointGraphLogic.ShowWindow();
        }
    }
}
