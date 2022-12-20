using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Reports;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelperLogics.Models.Calculations.CalculationsResults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.CalculationResult
{
    public class CalculationResultViewModel
    {
        public ICalculationResult SelectedResult { get; set; }
        public ICommand ShowIsoFieldCommand { get;}
        private ObservableCollection<ICalculationResult> calculationResults;
        private IEnumerable<INdm> ndms;
        private IReport isoFieldReport;


        public CalculationResultViewModel(IEnumerable<ICalculationResult> results, IEnumerable<INdm> ndmCollection)
        {
            ShowIsoFieldCommand = new RelayCommand(o=>ShowIsoField(), o=> !(SelectedResult is null) && SelectedResult.IsValid);
            //
            calculationResults = new ObservableCollection<ICalculationResult>();
            ndms = ndmCollection;
            foreach (var result in results)
            {
                calculationResults.Add(result);
            }
        }

        public ObservableCollection<ICalculationResult> CalculationResults
        {
            get
            {
                return calculationResults;
            }
        }   

        private void ShowIsoField()
        {
            IStrainMatrix strainMatrix = SelectedResult.LoaderResults.ForceStrainPair.StrainMatrix;
            var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(strainMatrix, ndms, ResultFuncFactory.GetResultFuncs());
            isoFieldReport = new IsoFieldReport(primitiveSets);
            isoFieldReport.Show();
        }
    }
}
