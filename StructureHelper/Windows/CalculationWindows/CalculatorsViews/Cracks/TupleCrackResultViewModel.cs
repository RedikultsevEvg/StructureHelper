using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class TupleCrackResultViewModel : ViewModelBase
    {
        IShowCrackIsoFieldsLogic showCrackIsoFieldsLogic => new ShowCrackIsoFieldsLogic();
        private TupleCrackResult crackResult;
        private RelayCommand showIsoFieldCommand;
        private IsoFieldReport isoFieldReport;

        public TupleCrackResult CrackResult => crackResult;
        public List<RebarCrackResult> RebarResults => crackResult.RebarResults;
        public RebarCrackResult SelectedResult { get; set; }
        public string WindowName => "Result of calculation of cracks for action " + crackResult.InputData.TupleName;
        public ICommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??= new RelayCommand(o =>
                {
                    showCrackIsoFieldsLogic.ShowIsoField(crackResult.RebarResults);
                });
            }
        }
        public TupleCrackResultViewModel(TupleCrackResult crackResult)
        {
            this.crackResult = crackResult;
        }
    }
}
