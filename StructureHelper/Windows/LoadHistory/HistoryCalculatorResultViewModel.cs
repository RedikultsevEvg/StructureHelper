using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace StructureHelper.Windows.LoadHistory
{
    public class HistoryCalculatorResultViewModel : ViewModelBase
    {
        private IHistoryCalculatorResult result;
        private RelayCommand showIsoFieldCommand;

        public List<SubStageResultViewModel> StagesResult { get; } = [];
        public SubStageResultViewModel? SelectedResult { get; set;}

        public ICommand ShowIsoFieldCommand
        {
            get
            {
                return showIsoFieldCommand ??= new RelayCommand(o =>
                {
                    SafetyProcessor.RunSafeProcess(ShowIsoField, "Error of showing of contours");
                }
                , o =>
                SelectedResult != null
                &&
                SelectedResult.IsValid
                );
            }
        }

        public HistoryCalculatorResultViewModel(IHistoryCalculatorResult result)
        {
            this.result = result;
            StagesResult.Clear();
            foreach (var stageResult in result.StagesResults)
            {
                foreach (var subStageResult in stageResult.SubStagesResults)
                {
                    StagesResult.Add(new SubStageResultViewModel(subStageResult));
                }
            }
        }

        private void ShowIsoField()
        {
            if (SelectedResult is null) { return; }
            ISubStageResult subStageResult = SelectedResult.Result;
            var logic = new ShowIsoFieldLogic()
            {
                Primitives = subStageResult.Primitives,
                StateCalcTermPair = subStageResult.StateCalcTermPair,
                StrainMatrix = subStageResult.ForcesTupleResult.LoaderResults.StrainMatrix
            };
            logic.ShowIsoField2D();
        }
    }
}
