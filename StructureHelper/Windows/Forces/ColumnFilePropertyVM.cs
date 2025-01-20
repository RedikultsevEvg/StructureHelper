using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ColumnFilePropertyVM : ViewModelBase, IDataErrorInfo
    {
        private IColumnFileProperty model;

        public ColumnFilePropertyVM(IColumnFileProperty model)
        {
            this.model = model;
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;
                if (columnName == nameof(Index))
                {
                    if (Index < 0)
                    {
                        error = "Index must be greater or equal to zero";
                    }
                }
                return error;
            }
        }

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string SearchingName
        {
            get => model.SearchingName;
            set
            {
                model.SearchingName = value;
                OnPropertyChanged(nameof(SearchingName));
            }
        }
        public int Index
        {
            get => model.Index + 1;
            set
            {
                model.Index = value - 1;
                OnPropertyChanged(nameof(Index));
            }
        }
        public double Factor
        {
            get => model.Factor;
            set
            {
                model.Factor = value;
                OnPropertyChanged(nameof(Factor));
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
