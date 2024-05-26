using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class CrackResultViewModel : ViewModelBase
    {
        IShowCrackIsoFieldsLogic showCrackIsoFieldsLogic => new ShowCrackIsoFieldsLogic();
        private CrackResult crackResult;
        private RelayCommand? showIsoFieldCommand;
        private RelayCommand? showRebarsCommand;

        public TupleCrackResult SelectedResult { get; set; }
        public List<TupleCrackResult> TupleResults => CrackResult.TupleResults;
        public ICommand ShowRebarsCommand
        {
            get
            {
                return showRebarsCommand ??= new RelayCommand(o =>
                {
                    var wnd = new TupleCrackResultView(SelectedResult);
                    wnd.ShowDialog();
                }, o => SelectedResult != null && SelectedResult.IsValid);
            }
        }

        public ICommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??= new RelayCommand(o =>
                {
                    showCrackIsoFieldsLogic.ShowIsoField(SelectedResult.RebarResults);
                }, o => SelectedResult != null && SelectedResult.IsValid);
            }
        }

        public CrackResult CrackResult => crackResult;

        public CrackResultViewModel(CrackResult crackResult)
        {
            this.crackResult = crackResult;
        }
    }
}
