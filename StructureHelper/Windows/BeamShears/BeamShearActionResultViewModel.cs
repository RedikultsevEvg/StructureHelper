using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionResultViewModel : ViewModelBase
    {
        private IBeamShearActionResult result;
        private RelayCommand showTraceCommand;
        private RelayCommand showDiagramCommand;
        private RelayCommand showGraphResultsCommand;

        public IBeamShearSectionLogicResult SelectedResult { get; set; }
        public List<IBeamShearSectionLogicResult> SectionResults => result.SectionResults;
        public ValidResultCounterVM ValidResultCounter { get; }

        public BeamShearActionResultViewModel(IBeamShearActionResult result)
        {
            this.result = result;
            ValidResultCounter = new(this.result.SectionResults);
        }

        public ICommand ShowTraceCommand => showTraceCommand ??= new RelayCommand(ShowTrace, o => SelectedResult != null);
        public ICommand ShowDiagramCommand => showDiagramCommand ??= new RelayCommand(Show2DDiagram, o => SelectedResult != null);
        public ICommand ShowGraphResultsCommand => showGraphResultsCommand ??= new RelayCommand(ShowGraphResults, o => SelectedResult != null);

        private void ShowGraphResults(object obj)
        {
            var window = new InclinedSectionViewerView(SelectedResult);
            window.ShowDialog();
        }

        private void ShowTrace(object obj)
        {
            if (SelectedResult.TraceLogger is null) { return; }
            var traceWindows = new TraceDocumentView(SelectedResult.TraceLogger);
            traceWindows.ShowDialog();
        }

        private void Show2DDiagram(object obj)
        {
            if (SelectedResult is null) { return; }
            var logic = new ShearDiagramLogic(result);
            logic.ShowWindow(SelectedResult.InputData.InclinedSection.StartCoord);
        }
    }
}
