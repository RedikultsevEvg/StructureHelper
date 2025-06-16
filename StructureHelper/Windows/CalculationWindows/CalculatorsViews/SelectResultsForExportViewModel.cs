using StructureHelper.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class SelectResultsForExportViewModel : OkCancelViewModelBase
    {
        private readonly SelectResultSettings resultSettings;

        public string Filename
        {
            get => resultSettings.Filename;
            set
            {
                resultSettings.Filename = value;
                OnPropertyChanged(nameof(Filename));
            }
        }

        public bool ExportValidResults
        {
            get => resultSettings.ExportValidResults;
            set
            {
                resultSettings.ExportValidResults = value;
                OnPropertyChanged(nameof(ExportValidResults));
            }
        }
        public bool ExportInValidResults
        {
            get => resultSettings.ExportInValidResults;
            set
            {
                resultSettings.ExportInValidResults = value;
                OnPropertyChanged(nameof(ExportInValidResults));
            }
        }
        public ValidResultCounterVM ValidResultCounter {get;}

        public SelectResultsForExportViewModel(SelectResultSettings resultSettings)
        {
            this.resultSettings = resultSettings;
            ValidResultCounter = new(this.resultSettings.Results);
        }
    }
}
