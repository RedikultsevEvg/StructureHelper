using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    public class ShowProgressViewModel : ViewModelBase
    {
        private double progressValue;
        private double maxValue;
        private double minValue;

        public string WindowTitle { get; set; }

        public double MinValue
        {
            get => minValue; set
            {
                minValue = value;
                OnPropertyChanged(nameof(MinValue));
            }
        }
        public double MaxValue
        {
            get => maxValue; set
            {
                maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        public double ProgressValue
        {
            get => progressValue;
            set
            {
                progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }
    }
}
