using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Services.Exports
{
    internal class ExportToFileService : IExportService
    {
        IExportToFileInputData inputData;
        IExportResultLogic logic;

        public ExportToFileService(IExportToFileInputData inputData, IExportResultLogic logic)
        {
            this.inputData = inputData;
            this.logic = logic;
        }
        public void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = inputData.Filter;
            saveFileDialog.Title = inputData.Title;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = saveFileDialog.FileName;
                // If the file name is not an empty string open it for saving.
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
            logic.FileName = fileName;
            logic.Export();
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
