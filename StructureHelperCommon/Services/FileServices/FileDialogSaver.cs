using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelperCommon.Services.FileServices
{
    public class FileDialogSaver : IFileDialogSaver
    {
        private const string saveCanceledByUser = "Saving was canceled by user";
        SaveFileResult result;
        public SaveDialogInputData? InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public SaveFileResult SaveFile()
        {
            CheckInput();
            result = new();
            using SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = InputData.FilterString;
            saveFileDialog.InitialDirectory = InputData.InitialDirectory;
            saveFileDialog.FilterIndex = InputData.FilterIndex;
            saveFileDialog.CheckFileExists = InputData.CheckFileExist;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                TraceLogger?.AddMessage($"User selected file {saveFileDialog.FileName}", TraceLogStatuses.Debug);
                result.FileName = saveFileDialog.FileName;
                return result;
            }
            else
            {
                TraceLogger?.AddMessage(saveCanceledByUser);
                result.IsValid = false;
                result.Description += saveCanceledByUser;
                return result;
            }
        }

        private void CheckInput()
        {
            if (InputData is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Input Data";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
