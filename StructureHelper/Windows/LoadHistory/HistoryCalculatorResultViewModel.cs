using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.LoadHistory
{
    public class HistoryCalculatorResultViewModel : ViewModelBase
    {
        private IHistoryCalculatorResult result;
        private RelayCommand showIsoFieldCommand;
        private RelayCommand showGrapsCommand;

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

        public ICommand ShowGraphsCommand
        {
            get
            {
                return showGrapsCommand ??= new RelayCommand(o =>
                {
                    SafetyProcessor.RunSafeProcess(ShowGraphs, "Error of showing of contours");
                }
                , o => result.IsValid
                &&
                result.StagesResults.Count(x => x.SubStagesResults.Any(y => y.IsValid)) > 0
                );
            }
        }

        private void ShowGraphs()
        {
            HistoryCalculatorResultDiagramFactory factory = new () { HistoryCalculatorResult = result};
            var arrayParameter = factory.GetCommonArray();
            var series = new Series(arrayParameter)
            {
                Name = "Forces and curvatures"
            };
            var vm = new GraphViewModel(new List<Series>() { series });
            var wnd = new GraphView(vm);
            wnd.ShowDialog();
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
