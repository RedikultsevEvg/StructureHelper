using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Exports;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace StructureHelper.Services.Exports
{
    public class ExportToFileService : IExportLogic
    {
        private IFileIOnputData inputData;
        private IExportToFileLogic exportLogic;

        public ExportToFileService(IFileIOnputData inputData, IExportToFileLogic exportLogic)
        {
            this.inputData = inputData;
            this.exportLogic = exportLogic;
        }
        public void Export()
        {
            SaveFileDialog dialog = new()
            {
                Filter = inputData.Filter,
                Title = inputData.Title
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dialog.FileName;
                if (filename != "")
                {
                    SaveFile(filename);
                }
            }
        }
        private void SaveFile(string filename)
        {
            if (File.Exists(filename))
            {
                DeleteFile(filename);
            }

            try
            {
                ExportFile(filename);
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = ErrorStrings.FileCantBeSaved + ex + filename,
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }
        private void DeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = ErrorStrings.FileCantBeDeleted + ex + filename,
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }
        private void ExportFile(string fileName)
        {
            exportLogic.FileName = fileName;
            exportLogic.Export();
            try
            {
                OpenFile(fileName);
            }
            catch (Exception) { }
        }
        private void OpenFile(string fileName)
        {
            var filopener = new Process();
            var startInfo = new ProcessStartInfo(fileName) { UseShellExecute = true };
            filopener.StartInfo = startInfo;
            filopener.Start();
        }
    }
}
