using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ForceFilePropertyVM : OkCancelViewModelBase
    {
        private IColumnedFileProperty model;

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
