using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.States;
using System.Collections.Generic;

namespace StructureHelper.Windows.UserControls.States
{
    public class StateCalcTermPairViewModel : ViewModelBase
    {
        private IStateCalcTermPair stateCalcTermPair;

        public List<LimitStates> LimitStatesCollection { get; set; } = [LimitStates.ULS, LimitStates.SLS];
        public List<CalcTerms> CalcTermsCollection { get; set; } = [CalcTerms.ShortTerm, CalcTerms.LongTerm];
        public LimitStates LimitState
        {
            get => stateCalcTermPair.LimitState;
            set
            {
                stateCalcTermPair.LimitState = value;
                OnPropertyChanged(nameof(LimitStates));
            }
        }

        public CalcTerms CalcTerm
        {
            get => stateCalcTermPair.CalcTerm;
            set
            {
                stateCalcTermPair.CalcTerm = value;
                OnPropertyChanged(nameof(CalcTerms));
            }
        }
        public StateCalcTermPairViewModel(IStateCalcTermPair stateCalcTermPair)
        {
            this.stateCalcTermPair = stateCalcTermPair;
        }
    }
}
