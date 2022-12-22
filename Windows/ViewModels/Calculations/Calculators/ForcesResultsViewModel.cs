using FieldVisualizer.Infrastructure.Commands;
using FieldVisualizer.ViewModels;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmCalculations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForcesResultsViewModel : ViewModelBase
    {
        private IForceCalculator forceCalculator;
        private IForcesResults forcesResults;
        private IEnumerable<INdmPrimitive> ndmPrimitives;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;

        public ForcesResult SelectedResult { get; set; }
        private RelayCommand showIsoFieldCommand;
        private RelayCommand exportToCSVCommand;
        private RelayCommand interpolateCommand;

        public IForcesResults ForcesResults
        {
            get => forcesResults;
        }

        public RelayCommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??
                (showIsoFieldCommand = new RelayCommand(o =>
                {
                    GetNdms();
                    ShowIsoField();
                }, o => (SelectedResult != null) && SelectedResult.IsValid));
            }
        }

        public RelayCommand ExportToCSVCommand
        {
            get
            {
                return exportToCSVCommand ??
                    (exportToCSVCommand = new RelayCommand(o =>
                    {

                    }
                    ));
            }
        }

        public RelayCommand InterpolateCommand
        {
            get
            {
                return interpolateCommand ??
                    (interpolateCommand = new RelayCommand(o =>
                    {
                        Interpolate();
                    }, o => SelectedResult != null));
            }
        }

        private void Interpolate()
        {
            int stepCount = 100;
            var calculator = InterpolateService.InterpolateForceCalculator(forceCalculator, SelectedResult.DesignForceTuple, stepCount);
            calculator.Run();
            var result = calculator.Result;
            if (result is null || result.IsValid == false)
            {
                MessageBox.Show(result.Desctription, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var vm = new ForcesResultsViewModel(calculator);
                var wnd = new ForceResultsView(vm);
                wnd.Show();
            }
        }

        public ForcesResultsViewModel(IForceCalculator forceCalculator)
        {
            this.forceCalculator = forceCalculator;
            this.forcesResults = this.forceCalculator.Result as IForcesResults;
            ndmPrimitives = this.forceCalculator.Primitives;
        }

        private void ShowIsoField()
        {
            IStrainMatrix strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
            var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ResultFuncFactory.GetResultFuncs());
            isoFieldReport = new IsoFieldReport(primitiveSets);
            isoFieldReport.Show();
        }

        private void GetNdms()
        {
            var limitState = SelectedResult.DesignForceTuple.LimitState;
            var calcTerm = SelectedResult.DesignForceTuple.CalcTerm;
            ndms = NdmPrimitivesService.GetNdms(ndmPrimitives, limitState, calcTerm);
        }
    }
}
