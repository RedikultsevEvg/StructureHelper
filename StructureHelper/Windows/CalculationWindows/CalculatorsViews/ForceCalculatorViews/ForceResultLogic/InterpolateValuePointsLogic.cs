using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class InterpolateValuePointsLogic
    {
        private InterpolationProgressLogic interpolationLogic;
        private ValuePointsInterpolateViewModel viewModel;
        private IResult result;
        private ValuePointsInterpolationInputData inputData;
        public ForcesTupleResult SelectedResult { get; set; }
        public IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }
        public ForceCalculator ForceCalculator { get; set; }


        public ILongProcessLogic ProgressLogic { get; set; }
        public ShowProgressLogic ShowProgressLogic { get; set; }

        public void InterpolateValuePoints()
        {
            var tuple = SelectedResult.DesignForceTuple ?? throw new StructureHelperException(ErrorStrings.NullReference + ": Design force combination");
            PrepareInputData(tuple);
            viewModel = new ValuePointsInterpolateViewModel(inputData);
            if (ShowDialog() == false) { return; };
            ShowDiagram(result);
        }

        private void PrepareInputData(IDesignForceTuple tuple)
        {
            inputData = new ValuePointsInterpolationInputData()
            {
                FinishDesignForce = tuple.Clone() as IDesignForceTuple,
                LimitState = tuple.LimitState,
                CalcTerm = tuple.CalcTerm,
            };
            inputData.PrimitiveBases.AddRange(PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(NdmPrimitives));
        }

        private bool ShowDialog()
        {
            var wnd = new ValuePointsInterpolateView(viewModel);
            wnd.ShowDialog();
            if (wnd.DialogResult != true) { return false; }
            interpolationLogic = new InterpolationProgressLogic(ForceCalculator, viewModel.ForceInterpolationViewModel.Result);
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
            if (result is not IForcesResults)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(IForcesResults), result));
            }
            var tupleResult = result as IForcesResults;
            var pointGraphLogic = new ShowValuePointDiagramLogic()
            {
                Calculator = interpolationLogic.InterpolateCalculator,
                PrimitiveLogic = viewModel.PrimitiveLogic,
                ValueDelegatesLogic = viewModel.ValueDelegatesLogic,
                TupleList = tupleResult.ForcesResultList
            };
            pointGraphLogic.ShowWindow();
        }
    }
}
