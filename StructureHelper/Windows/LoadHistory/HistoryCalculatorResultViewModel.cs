using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Services.FileServices;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.LoadHistory
{
    public class HistoryCalculatorResultViewModel : ViewModelBase
    {
        private IHistoryCalculatorResult result;
        private List<ISubStageResult> subStageResultCollection;
        private RelayCommand showIsoFieldCommand;
        private RelayCommand showPrestrainFieldCommand;
        private RelayCommand showGrapsCommand;
        private RelayCommand showDetailsCommand;
        private RelayCommand exportToExcelCommand;
        private RelayCommand copyToClipboardCommand;

        public List<SubStageResultViewModel> StagesResult { get; } = [];
        public SubStageResultViewModel? SelectedResult { get; set;}
        public ValidResultCounterVM ValidResultCounter { get; }

        public ICommand ShowDetailsCommand
        {
            get
            {
                return showDetailsCommand ??= new RelayCommand(o => ShowDetails(), o => SelectedResult != null);
            }
        }

        public ICommand ExportToExcelCommand
        {
            get
            {
                return exportToExcelCommand ??= new RelayCommand(o => ExportToExcel(), o => StagesResult.Count > 0);
            }
        }

        public ICommand CopyToClipboardCommand
        {
            get
            {
                return copyToClipboardCommand ??= new RelayCommand(o => CopyToClipboard(), o => StagesResult.Count > 0);
            }
        }

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

        public ICommand ShowPrestrainFieldCommand
        {
            get
            {
                return showPrestrainFieldCommand ??= new RelayCommand(o =>
                {
                    SafetyProcessor.RunSafeProcess(ShowPrestrainField, "Error of showing of contours");
                }
                , o =>
                SelectedResult != null
                &&
                SelectedResult.IsValid
                );
            }
        }

        private void ShowPrestrainField()
        {
            if (SelectedResult is null) { return; }
            ISubStageResult subStageResult = SelectedResult.Result;
            var logic = new ShowIsoFieldLogic()
            {
                Primitives = subStageResult.Primitives,
                StateCalcTermPair = subStageResult.StateCalcTermPair,
                StrainMatrix = subStageResult.ForcesTupleResult.LoaderResults.StrainMatrix
            };
            logic.ShowPrestrainField2D();
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
                subStageResultCollection.Count(x => x.IsValid == true) > 0
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

        private void ShowDetails()
        {
            if (SelectedResult is null)
            {
                return;
            }
            var viewModel = new HistoryResultDetailsViewModel(SelectedResult.Result);
            var window = new HistoryResultDetailsView(viewModel);
            window.ShowDialog();
        }

        private void ExportToExcel()
        {
            var saver = new FileDialogSaver
            {
                InputData = new SaveDialogInputData
                {
                    FilterString = "Excel workbook (*.xlsx)|*.xlsx"
                }
            };
            SaveFileResult saveResult = saver.SaveFile();
            if (saveResult.IsValid == false || string.IsNullOrWhiteSpace(saveResult.FileName))
            {
                return;
            }

            try
            {
                XlsxTableWriter.Write(
                    saveResult.FileName,
                    "Load history",
                    HistoryResultTableBuilder.Headers,
                    HistoryResultTableBuilder.GetRows(StagesResult.Select(x => x.Result)));
            }
            catch (Exception ex)
            {
                SafetyProcessor.ShowMessage("Error of exporting of load history results", $"{ex}");
            }
        }

        private void CopyToClipboard()
        {
            IEnumerable<ISubStageResult> results = SelectedResult is null
                ? StagesResult.Select(x => x.Result)
                : new[] { SelectedResult.Result };

            try
            {
                Clipboard.SetText(HistoryResultTableBuilder.ToTabSeparatedText(results));
            }
            catch (Exception ex)
            {
                SafetyProcessor.ShowMessage("Error of copying of load history results", $"{ex}");
            }
        }

        public HistoryCalculatorResultViewModel(IHistoryCalculatorResult result)
        {
            this.result = result;
            subStageResultCollection = result.StagesResults.SelectMany(x => x.SubStagesResults).ToList();
            ValidResultCounter = new(subStageResultCollection);
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
            logic.ShowStressStrainField2D();
        }
    }
}
