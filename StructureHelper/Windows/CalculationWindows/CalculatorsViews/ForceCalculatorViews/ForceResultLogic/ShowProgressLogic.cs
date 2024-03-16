using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowProgressLogic
    {
        private ShowProgressViewModel progressViewModel;
        private ShowProgressView wndProgress;
        private bool result;
        private string resultDescription;
        private ILongProcessLogic processLogic;

        public string WindowTitle { get; set; }

        public void Show()
        {
            progressViewModel = new()
            {
                MinValue = 0,
                MaxValue = processLogic.StepCount,
                ProgressValue = 0,
                WindowTitle = WindowTitle
            };

            wndProgress = new ShowProgressView(progressViewModel);
            wndProgress.Loaded += RunCalc;
            wndProgress.ShowDialog();
        }

        public Action ShowResult { get; set; }

        public ShowProgressLogic(ILongProcessLogic processLogic)
        {
            this.processLogic = processLogic;
            processLogic.SetProgress = ShowProgressResult;
        }

        public void ShowProgressResult(int progressValue)
        {
            progressViewModel.MaxValue = processLogic.StepCount;
            progressViewModel.ProgressValue = progressValue;
        }

        private void RunCalc(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new();
            worker.DoWork += processLogic.WorkerDoWork;
            worker.ProgressChanged += WorkerProgressChanged;
            worker.RunWorkerCompleted += WorkerRunWorkCompleted;
            worker.RunWorkerAsync();
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            processLogic.WorkerProgressChanged(sender, e);
        }

        private void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                processLogic.WorkerRunWorkCompleted(sender, e);
                wndProgress.Close();
                result = processLogic.Result;
                if (result == true)
                {
                    ShowResult?.Invoke();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(resultDescription, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw new StructureHelperException(ex);
            }
        }
    }
}
