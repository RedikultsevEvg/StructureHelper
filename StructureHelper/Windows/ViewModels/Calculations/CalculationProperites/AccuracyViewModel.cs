using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelper.Windows.ViewModels
{
    internal class AccuracyViewModel : ViewModelBase
    {
        public IAccuracy Accuracy { get; }


        public double IterationAccuracy
        {
            get => Accuracy.IterationAccuracy;
            set
            {
                Accuracy.IterationAccuracy = value;
                OnPropertyChanged(nameof(IterationAccuracy));
            }
        }
        public int MaxIterationCount
        {
            get => Accuracy.MaxIterationCount;
            set
            {
                Accuracy.MaxIterationCount = value;
                OnPropertyChanged(nameof(MaxIterationCount));
            }
        }
        public AccuracyViewModel(IAccuracy accuracy)
        {
            Accuracy = accuracy;
        }

    }
}
