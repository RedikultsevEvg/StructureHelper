using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.ComponentModel;

namespace StructureHelper.Windows.LoadHistory
{
    public class HistoryCalculatorViewModel : OkCancelViewModelBase
    {
        private string name;

        public ConstructionStagesViewModel ConstructionStagesViewModel { get; }
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
            ConstructionStagesViewModel = new(sourceCalculator.InputData.ConstructionStages, repository);
        }

    }
}
