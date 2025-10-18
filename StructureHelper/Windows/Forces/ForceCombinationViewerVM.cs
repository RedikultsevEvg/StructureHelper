using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ForceCombinationViewerVM : ViewModelBase
    {
        private IForceAction model;
        private LimitStates limitState = LimitStates.ULS;
        private CalcTerms calcTerm = CalcTerms.ShortTerm;
        private RelayCommand refreshCommand;

        public List<LimitStates> LimitStateList { get; set; } = [LimitStates.ULS, LimitStates.SLS];
        public List<CalcTerms> CalcTermList { get; set; } = [CalcTerms.LongTerm, CalcTerms.ShortTerm];
        public LimitStates LimitState
        {
            get => limitState;
            set
            {
                limitState = value;
                OnPropertyChanged(nameof(limitState));
            }
        }

        private void Refresh()
        {
            Combinations.Clear();
            var combinations = model.GetCombinations();
            foreach (var combination in combinations)
            {
                var combinationList = combination.DesignForces.Where(x => x.LimitState == limitState && x.CalcTerm == calcTerm).ToList();
                combinationList.ForEach(x => Combinations.Add(x));
            }
        }

        public CalcTerms CalcTerm
        {
            get => calcTerm;
            set
            {
                calcTerm = value;
                OnPropertyChanged(nameof (calcTerm));
            }
        }

        public ICommand RefreshCommand =>
            refreshCommand ??=
            new RelayCommand(o => SafetyProcessor.RunSafeProcess(Refresh, "Refresh combination errror"));

        public ObservableCollection<IDesignForceTuple> Combinations { get; } = [];

        public ForceCombinationViewerVM(IForceAction model)
        {
            this.model = model;
            Refresh();
        }
    }
}
