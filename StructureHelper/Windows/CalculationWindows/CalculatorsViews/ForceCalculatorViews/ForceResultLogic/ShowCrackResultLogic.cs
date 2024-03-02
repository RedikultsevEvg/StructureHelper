using LoaderCalculator;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowCrackResultLogic
    {
        private CrackForceCalculator calculator;

        public static GeometryNames GeometryNames => ProgramSetting.GeometryNames;
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IForceTuple ForceTuple { get; set; }
        public IEnumerable<INdmPrimitive> ndmPrimitives { get; set; }
        public void Show(IDesignForceTuple finishDesignTuple)
        {
            var viewModel = new InterpolateTuplesViewModel(finishDesignTuple, null);
            viewModel.ForceInterpolationViewModel.StepCountVisible = false;
            var wndTuples = new InterpolateTuplesView(viewModel);
            wndTuples.ShowDialog();
            if (wndTuples.DialogResult != true) return;
            var startDesignTuple = viewModel.ForceInterpolationViewModel.StartDesignForce.ForceTuple;
            var endDesignTuple = viewModel.ForceInterpolationViewModel.FinishDesignForce.ForceTuple;
            FindCrackFactor(endDesignTuple, startDesignTuple);
        }

        private void FindCrackFactor(IForceTuple finishDesignTuple, IForceTuple startDesignTuple)
        {
            calculator = new CrackForceCalculator();
            calculator.TraceLogger = new ShiftTraceLogger();
            calculator.StartTuple = startDesignTuple;
            calculator.EndTuple = finishDesignTuple;
            calculator.NdmCollection = NdmPrimitivesService.GetNdms(ndmPrimitives, LimitState, CalcTerm);
            calculator.Run();
            var result = (CrackForceResult)calculator.Result;
            if (result.IsValid)
            {
                ShowTraceResult();
                //ShowResult(result);
            }
            else
            {
                ShowError(result);
            }
        }

        private static void ShowError(CrackForceResult result)
        {
            var errorVM = new ErrorProcessor()
            {
                ShortText = "Error apeared while crack calculate",
                DetailText = result.Description
            };
            var wnd = new ErrorMessage(errorVM);
            wnd.ShowDialog();
        }

        private static void ShowResult(CrackForceResult result)
        {
            //var softLogic = new ExponentialSofteningLogic() { ForceRatio = result.ActualFactor };
            string message = string.Empty;
            if (result.IsSectionCracked)
            {
                message += $"Actual crack factor {result.FactorOfCrackAppearance}\n";
                message += $"Softening crack factor PsiS={result.PsiS}\n";
                message += $"{GeometryNames.MomFstName}={result.TupleOfCrackAppearance.Mx},\n {GeometryNames.MomSndName}={result.TupleOfCrackAppearance.My},\n {GeometryNames.LongForceName}={result.TupleOfCrackAppearance.Nz}";
            }
            else
            {
                message += "Cracks are not apeared";
            }
            MessageBox.Show(
                message,
                "Crack results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ShowTraceResult()
        {
            if (calculator.TraceLogger is not null)
            {
                var wnd = new TraceDocumentView(calculator.TraceLogger.TraceLoggerEntries);
                wnd.ShowDialog();
            }
        }
    }
}
