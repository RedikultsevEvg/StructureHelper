using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Services.FileServices
{
    public class FileOpener : IFileDialogOpener
    {
        private const string fileSelectionWasCanceled = "File selection was cancelled by user";
        OpenFileResult? result;
        IShiftTraceLogger? traceLogger;
        public OpenFileInputData? InputData { get; private set; }
        public FileOpener(OpenFileInputData inputData)
        {
            InputData = inputData;
        }
        public OpenFileResult OpenFile()
        {
            PrepareNewResult();
            CheckInputData();
            traceLogger = InputData.TraceLogger;
            ShowOpenFileDialog();
            return result;
        }

        private void ShowOpenFileDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set filter options and filter index
                openFileDialog.Filter = InputData.FilterString;
                openFileDialog.FilterIndex = InputData.FilterIndex;
                openFileDialog.Multiselect = InputData.MultiSelect;
                openFileDialog.Title = InputData.Title;

                // Show the dialog and get result
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the selected file
                    string selectedFilePath = openFileDialog.FileName;
                    traceLogger?.AddMessage($"File {selectedFilePath} is selected by user", TraceLogStatuses.Debug);
                    result.FilePath = selectedFilePath;
                }
                else
                {
                    result.IsValid = false;
                    result.Description = fileSelectionWasCanceled;
                    traceLogger?.AddMessage(fileSelectionWasCanceled, TraceLogStatuses.Debug);
                    Console.WriteLine(fileSelectionWasCanceled);
                }
            }
        }
        private void CheckInputData()
        {
            if (InputData is null)
            {
                result.IsValid = false;
                string message = ErrorStrings.ParameterIsNull + ": Input Data";
                result.Description = message;
                throw new StructureHelperException(message);
            }
        }
        private void PrepareNewResult()
        {
            result = new()
            {
                IsValid = true
            };
        }
    }
}
