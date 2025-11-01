using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Cracking;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class CrackResultViewModel : ViewModelBase
    {
        IShowCrackIsoFieldsLogic showCrackIsoFieldsLogic => new ShowCrackIsoFieldsLogic();
        private ICrackResult resultModel;
        private RelayCommand? showIsoFieldCommand;
        private RelayCommand? showRebarsCommand;
        private RelayCommand exportToCSVCommand;

        public TupleCrackResult SelectedResult { get; set; }
        public List<ITupleCrackResult> TupleResults => CrackResult.TupleResults;
        public ValidResultCounterVM ValidResultCounter { get; }
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

        public ICrackResult CrackResult => resultModel;

        public CrackResultViewModel(ICrackResult crackResult)
        {
            this.resultModel = crackResult;
            ValidResultCounter = new(resultModel.TupleResults);
        }

        public ICommand ExportToCSVCommand => exportToCSVCommand ??= new RelayCommand(o => { ExportToCSV(); });
        private void ExportToCSV()
        {
            var inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Csv);
            var logic = new ExportCrackResultToCSVLogic(resultModel);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }
    }
}
