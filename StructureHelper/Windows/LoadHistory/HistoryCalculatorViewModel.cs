using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.LoadHistory
{
    public class HistoryCalculatorViewModel : OkCancelViewModelBase
    {
        private string name;

        public ConstructionStagesViewModel ConstructionStagesViewModel { get; }
        public AccuracyViewModel AccuracyViewModel { get; }
        public IHistoryCalculator HistoryCalculator { get; }

        public string Name
        {
            get => HistoryCalculator.Name;
            set
            {
                HistoryCalculator.Name = value;
                Refresh();
            }
        }

        public string WindowName => $"Load History Calculator: {Name}";

        private void Refresh()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(WindowName));
        }

        public HistoryCalculatorViewModel(IHistoryCalculator sourceCalculator, ICrossSectionRepository repository)
        {
            HistoryCalculator = sourceCalculator;
            AccuracyViewModel = new AccuracyViewModel(sourceCalculator.InputData.Accuracy);
            ConstructionStagesViewModel = new(sourceCalculator.InputData.ConstructionStages, repository);
        }

        public override void OkAction()
        {
            if (AccuracyViewModel.TryApply() == false)
            {
                MessageBox.Show(
                    AccuracyViewModel.ValidationMessage,
                    "Check accuracy",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }

    }
}
