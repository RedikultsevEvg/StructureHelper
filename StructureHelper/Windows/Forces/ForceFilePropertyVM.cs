using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.FileServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class ForceFilePropertyVM : OkCancelViewModelBase
    {
        private IColumnedFileProperty model;
        private RelayCommand openFileCommand;

        public ForceFilePropertyVM(IColumnedFileProperty model)
        {
            this.model = model;
            foreach (var item in model.ColumnProperties)
            {
                ColumnProperties.Add(new ColumnFilePropertyVM(item));
            }
        }

        public int SkipRowBeforeHeaderCount
        {
            get => model.SkipRowBeforeHeaderCount;
            set
            {
                model.SkipRowBeforeHeaderCount = value;
                OnPropertyChanged(nameof(SkipRowBeforeHeaderCount));
            }
        }
        public int SkipRowHeaderCount
        {
            get => model.SkipRowHeaderCount;
            set
            {
                model.SkipRowHeaderCount = value;
                OnPropertyChanged(nameof(SkipRowHeaderCount));
            }
        }
        public double GlobalFactor
        {
            get => model.GlobalFactor;
            set
            {
                model.GlobalFactor = value;
                OnPropertyChanged(nameof(GlobalFactor));
            }
        }
        public string FilePath
        {
            get => model.FilePath;
            set
            {
                model.FilePath = value;
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

        public ObservableCollection<ColumnFilePropertyVM> ColumnProperties { get; set; } = new();

        public IColumnedFileProperty Model
        {
            get
            {
                return model;
            }
        }
    }
}
