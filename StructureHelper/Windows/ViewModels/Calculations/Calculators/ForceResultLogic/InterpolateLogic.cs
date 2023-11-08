using LoaderCalculator;
using LoaderCalculator.Data.ResultData;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmCalculations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    internal class InterpolateLogic
    {
        private InterpolationProgressViewModel progressViewModel;
        private InterpolationProgressView wndProgress;
        private IForceCalculator forceCalculator;
        private IDesignForceTuple finishDesignTuple;
        private IDesignForceTuple startDesignTuple;
        private int stepCount;
        private IResult result;
        private IForceCalculator interpolateCalculator;

        public void Show(IDesignForceTuple finishTuple, IForceCalculator forceCalculator)
        {
            this.forceCalculator = forceCalculator;
            var viewModel = new InterpolateTuplesViewModel(finishTuple, null);
            var wndTuples = new InterpolateTuplesView(viewModel);
            wndTuples.ShowDialog();
            if (wndTuples.DialogResult != true) return;

            startDesignTuple = viewModel.StartDesignForce;
            finishDesignTuple = viewModel.FinishDesignForce;
            stepCount = viewModel.StepCount;
            progressViewModel = new()
            {
                MinValue = 0,
                MaxValue = stepCount,
                ProgressValue = 0
            };

            wndProgress =new InterpolationProgressView(progressViewModel);
            wndProgress.Loaded += RunCalc;
            wndProgress.ShowDialog();
        }

        private void RunCalc(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new();
            worker.DoWork += WorkerDoWork;
            worker.ProgressChanged += WorkerProgressChanged;
            worker.RunWorkerCompleted += WorkerRunWorkCompleted;
            worker.RunWorkerAsync();
        }

        private void ShowProgressResult(IResult result)
        {
            if (result is ForcesResults)
            {
                var forceResult = result as ForcesResults;
                progressViewModel.ProgressValue = forceResult.ForcesResultList.Count();
            }
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            interpolateCalculator = InterpolateService.InterpolateForceCalculator(forceCalculator, finishDesignTuple, startDesignTuple, stepCount);
            interpolateCalculator.ActionToOutputResults = ShowProgressResult;
            interpolateCalculator.Run();
            result = interpolateCalculator.Result;
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                wndProgress.Close();

                if (result is null || result.IsValid == false)
                {
                    System.Windows.Forms.MessageBox.Show(result.Description, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var vm = new ForcesResultsViewModel(interpolateCalculator);
                    var wnd = new ForceResultsView(vm);
                    wnd.ShowDialog();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
