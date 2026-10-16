using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperLogics.Models.LoadHistoryCalculator;

namespace StructureHelper.Windows.LoadHistory
{
    internal class ConstructionStageViewModel : OkCancelViewModelBase
    {
        private const int minSubStepNumber = 3;
        private readonly IConstructionStage constructionStage;
        public ForceTupleVM ForceTuple { get; }

        public string Name
        {
            get => constructionStage.Name;
            set
            {
                constructionStage.Name = value;
                Refresh();
            }
        }

        public int SubStemNumber
        {
            get => constructionStage.SubStepNumber;
            set
            {
                constructionStage.SubStepNumber = value;
            }
        }

        public string WindowName => "Costruction stage: " + constructionStage.Name;
        public string SubStepNumberName => $"Sub step number (min {minSubStepNumber})";

        private void Refresh()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(WindowName));
            OnPropertyChanged(nameof(SubStemNumber));
        }

        public ConstructionStageViewModel(IConstructionStage constructionStage)
        {
            this.constructionStage = constructionStage;
            ForceTuple = new(this.constructionStage.ForceTuple);
        }
    }
}
