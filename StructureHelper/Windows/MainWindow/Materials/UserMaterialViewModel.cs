using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow.Materials
{
    public class UserMaterialViewModel : HeadMaterialBaseViewModel
    {
        IUserMaterial userMaterial;
        private RelayCommand openFileCommand;


        public string FilePath
        {
            get => userMaterial.FilePath;
            set
            {
                userMaterial.FilePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public ICommand OpenFileCommand => openFileCommand ??= new RelayCommand(o => OpenFileMethod());

        private void OpenFileMethod()
        {
            var result = GetFilePath();
            if (result.IsValid == false)
            {
                return;
            }
            FilePath = result.FilePath;
        }

        private OpenFileResult GetFilePath()
        {
            var inputData = new OpenFileInputData()
            {
                FilterString = "MS Excel file (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                TraceLogger = null
            };
            var fileDialog = new FileOpener(inputData);
            var fileDialogResult = fileDialog.OpenFile();
            return fileDialogResult;
        }

        public UserMaterialViewModel(IHeadMaterial headMaterial) : base(headMaterial)
        {
            if (headMaterial.HelperMaterial is not IUserMaterial userHelperMaterial)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(headMaterial.HelperMaterial) + ": helper material object is not user material");
            }
            this.userMaterial = userHelperMaterial;
        }
    }
}
