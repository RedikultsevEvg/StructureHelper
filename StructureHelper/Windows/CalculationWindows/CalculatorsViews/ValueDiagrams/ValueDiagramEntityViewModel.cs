using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramEntityViewModel : OkCancelViewModelBase
    {
        private IValueDiagramEntity valueDiagramEntity;

        public bool IsTaken
        {
            get => valueDiagramEntity.IsTaken;
            set
            {
                valueDiagramEntity.IsTaken = value;
                OnPropertyChanged(nameof(IsTaken));
            }
        }

        public string Name
        {
            get => valueDiagramEntity.Name;
            set
            {
                valueDiagramEntity.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ValueDiagramViewModel ValueDiagram { get; set; }
        public ValueDiagramEntityViewModel(IValueDiagramEntity valueDiagramEntity)
        {
            this.valueDiagramEntity = valueDiagramEntity;
            ValueDiagram = new(valueDiagramEntity.ValueDiagram);
        }
    }
}
