using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
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

        private void ExportToExcel(object obj)
        {
            if (SelectedResult is null) { return; }
            var inputData = new ExportToFileInputData
            {
                Filter = "csv |*.csv",
                Title = "Save in *.csv File"
            };
            var logic = new ExportActionResultToCSVLogic(SelectedResult);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        private void Show2DDiagram(object obj)
        {
            if (SelectedResult is null) { return; }
            var logic = new ShearDiagramLogic(SelectedResult);
            logic.ShowWindow(0.0);
        }

        private void ShowSectionResults(object commandParameter)
        {
            if (SelectedResult is null) { return; }
            Window window = new BeamShearActionResultView(SelectedResult);
            window.ShowDialog();
        }
    }
}
