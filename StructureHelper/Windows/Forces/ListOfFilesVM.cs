using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ListOfFilesVM : SelectItemVM<IForceFileProperty>
    {
        private ICommand openFileCommand;
        public IShiftTraceLogger? TraceLogger;

        public ICommand FileOpen => openFileCommand ?? (
                    openFileCommand = new RelayCommand(param =>
                    {
                        OpenFileMethod(param);
                    }
                    ));

        private void OpenFileMethod(object param)
        {
            var result = GetFilePath();
            if (result.IsValid == false)
            {
                return;
            }
            ForceFileProperty fileProperty = new()
            {
                FilePath = result.FilePath,
            };
            Collection.Add(fileProperty);
            Refresh();
        }

        public ListOfFilesVM(List<IForceFileProperty> collection) : base(collection)
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
