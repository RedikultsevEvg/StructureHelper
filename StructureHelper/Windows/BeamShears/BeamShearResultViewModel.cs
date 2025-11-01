using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearResultViewModel : ViewModelBase
    {
        private IBeamShearCalculatorResult result;
        private RelayCommand showSectionResultsCommand;
        private RelayCommand showDiagramCommand;
        private RelayCommand exportToExcelCommand;
        private RelayCommand showGraphResultsCommand;

        public IBeamShearActionResult SelectedResult { get; set; }
        public List<IBeamShearActionResult> ActionResults => result.ActionResults;
        public ValidResultCounterVM ValidResultCounter { get; }

        public BeamShearResultViewModel(IBeamShearCalculatorResult result)
        {
            this.result = result;
            ValidResultCounter = new(this.result.ActionResults);
        }

        public ICommand ShowSectionResultsCommand => showSectionResultsCommand ??= new RelayCommand(ShowSectionResults, o=>SelectedResult != null);


        public ICommand ShowDiagramCommand => showDiagramCommand ??= new RelayCommand(Show2DDiagram, o=>SelectedResult != null);
        public ICommand ExportToExcelCommand => exportToExcelCommand ??= new RelayCommand(ExportToExcel, o=>SelectedResult != null);
        public ICommand ShowGraphResultsCommand => showGraphResultsCommand ??= new RelayCommand(ShowGraphResults, o => SelectedResult != null);

        private void ShowGraphResults(object obj)
        {
            if (SelectedResult is null) {return; }
            if (SelectedResult.SectionResults is null) {return; }
            var sectionResult = SelectedResult.SectionResults
            .OrderByDescending(x => x.FactorOfUsing)
            .FirstOrDefault();
            var window = new InclinedSectionViewerView(sectionResult);
            window.ShowDialog();
        }

        private void ExportToExcel(object obj)
        {
            if (SelectedResult is null) { return; }
            var logic = new ExportActionResultToCSVLogic(SelectedResult);
            var exportService = new ExportToFileService(FileInputDataFactory.GetFileIOInputData(FileInputDataType.Csv), logic);
            exportService.Export();
        }

        private void Show2DDiagram(object obj)
        {
            if (SelectedResult is null) { return; }
            if (SelectedResult.SectionResults is null) { return; }
            var sectionResult = SelectedResult.SectionResults
            .OrderByDescending(x => x.FactorOfUsing)
            .FirstOrDefault();
            var logic = new ShearDiagramLogic(SelectedResult);
            logic.ShowWindow(sectionResult.ResultInputData.InclinedSection.StartCoord);
        }

        private void ShowSectionResults(object commandParameter)
        {
            if (SelectedResult is null) { return; }
            Window window = new BeamShearActionResultView(SelectedResult);
            window.ShowDialog();
        }
    }
}
