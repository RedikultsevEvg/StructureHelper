using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ListOfFilesVM : SelectItemVM<IColumnedFileProperty>
    {
        private ICommand openFileCommand;
        public IShiftTraceLogger? TraceLogger;
        private ICommand showSettingsCommand;
        private IUpdateStrategy<IColumnedFileProperty> updateStrategy;
        private ICommand showDocumentCommand;

        public ICommand FileOpen => openFileCommand ?? (
                    openFileCommand = new RelayCommand(param =>
                    {
                        OpenFileMethod(param);
                    }
                    ));

        public ICommand ShowSettings => showSettingsCommand ?? (
            showSettingsCommand = new RelayCommand(param =>
            {
                ShowSettingsMethod(param);
            },o => SelectedItem is not null
            ));

        public ICommand ShowDocument => showDocumentCommand ?? (
            showDocumentCommand = new RelayCommand(param =>
            {
                SafetyProcessor.RunSafeProcess(ShowDocumentMethod, "Error of opening of settings");
                }, o => SelectedItem is not null
            ));

        private void ShowDocumentMethod()
        {
            if (SelectedItem is null) { return; }
        }

        private void ShowSettingsMethod(object param)
        {
            SafetyProcessor.RunSafeProcess(OpenSettingsWindow, "Error of opening of settings");
        }

        private void OpenSettingsWindow()
        {
            if (SelectedItem is null) { return; }
            var clone = (IColumnedFileProperty)SelectedItem.Clone();
            var wnd = new ForceFilePropertyView(SelectedItem);
            wnd.ShowDialog();
            if (! (bool)wnd.DialogResult)
            {
                updateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
                updateStrategy.Update(SelectedItem, clone);
            }
        }

        private void OpenFileMethod(object param)
        {
            var result = GetFilePath();
            if (result.IsValid == false)
            {
                return;
            }
            IColumnedFileProperty fileProperty = FilePropertyFactory.GetForceFileProperty(FilePropertyType.Forces);
            fileProperty.FilePath = result.FilePath;
            Collection.Add(fileProperty);
            Refresh();
        }

        public ListOfFilesVM(List<IColumnedFileProperty> collection) : base(collection)
        {
        }

        private OpenFileResult GetFilePath()
        {
            var inputData = new OpenFileInputData()
            {
                FilterString = "MS Excel file (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                TraceLogger = TraceLogger
            };
            var fileDialog = new FileOpener(inputData);
            var fileDialogResult = fileDialog.OpenFile();
            return fileDialogResult;
        }
    }
}
