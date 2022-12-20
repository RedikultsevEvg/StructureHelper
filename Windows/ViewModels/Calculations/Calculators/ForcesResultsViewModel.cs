using FieldVisualizer.Infrastructure.Commands;
using FieldVisualizer.ViewModels;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ICommand showIsoFieldCommand;

        public IForcesResults ForcesResults
        {
            get => forcesResults;
        }

        public ICommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??
                (
                showIsoFieldCommand = new RelayCommand(o =>
                {
                    GetNdms();
                    ShowIsoField();
                }, o => (SelectedResult != null) && SelectedResult.IsValid));
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
