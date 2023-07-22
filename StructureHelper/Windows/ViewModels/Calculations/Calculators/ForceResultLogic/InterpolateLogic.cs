using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmCalculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    internal class InterpolateLogic
    {
        public void Show(IDesignForceTuple finishDesignTuple, IForceCalculator forceCalculator)
        {
            IDesignForceTuple startDesignTuple;
            var viewModel = new InterpolateTuplesViewModel(finishDesignTuple, null, 100);
            var wndTuples = new InterpolateTuplesView(viewModel);
            wndTuples.ShowDialog();
            if (wndTuples.DialogResult != true) return;
            startDesignTuple = viewModel.StartDesignForce;
            finishDesignTuple = viewModel.FinishDesignForce;
            int stepCount = viewModel.StepCount;
            var calculator = InterpolateService.InterpolateForceCalculator(forceCalculator, finishDesignTuple, startDesignTuple, stepCount);
            calculator.Run();
            var result = calculator.Result;
            if (result is null || result.IsValid == false)
            {
                MessageBox.Show(result.Description, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var vm = new ForcesResultsViewModel(calculator);
                var wnd = new ForceResultsView(vm);
                wnd.ShowDialog();
            }
        }
    }
}
