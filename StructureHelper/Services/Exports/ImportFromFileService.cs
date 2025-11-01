using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Exports;
using System;
using System.IO;
using System.Windows.Forms;

namespace StructureHelper.Services.Exports
{
    internal class ImportFromFileService : IImportFromFileLogic
    {
        private IFileIOnputData inputData;
        private IImportFromFileLogic importLogic;

        public string FileName {get; set; }
        public ImportFromFileService(IFileIOnputData inputData, IImportFromFileLogic importLogic)
        {
            this.inputData = inputData;
            this.importLogic = importLogic;
        }

        public void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = inputData.Filter,
                Title = inputData.Title
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dialog.FileName;
                if (filename != "")
                {
                    OpenFile(filename);
                }
            }
        }

        private void OpenFile(string filename)
        {
            CheckIfFileExist(filename);
            ImportFromFile(filename);
        }

        private void ImportFromFile(string filename)
        {
            try
            {
                importLogic.FileName = filename;
                importLogic.Import();
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = ErrorStrings.FileCantBeOpened + ": " + filename,
                    DetailText = $"File: {filename} can be opened \n {ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }

        private static void CheckIfFileExist(string filename)
        {
            if (!File.Exists(filename))
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = ErrorStrings.FileDoesNotExsist + ": " + filename,
                    DetailText = $"File {filename} does not exist"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }
    }
}
