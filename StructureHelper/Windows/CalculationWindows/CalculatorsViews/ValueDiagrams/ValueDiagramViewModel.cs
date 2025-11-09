using StructureHelper.Infrastructure;
using StructureHelper.Windows.Shapes;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramViewModel : ViewModelBase
    {
        private IValueDiagram valueDiagram;

        public int StepNumber
        {
            get => valueDiagram.StepNumber;
            set
            {
                valueDiagram.StepNumber = value;
                OnPropertyChanged(nameof(StepNumber));
            }
        }
        public Point2DRangeViewModel Point2DRange { get; set; }

        public ValueDiagramViewModel(IValueDiagram valueDiagram)
        {
            this.valueDiagram = valueDiagram;
            Point2DRange = new(valueDiagram.Point2DRange);
        }
    }
}
