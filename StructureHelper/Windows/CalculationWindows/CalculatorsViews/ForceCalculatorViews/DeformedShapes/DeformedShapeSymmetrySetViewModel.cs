using StructureHelper.Infrastructure;
using System.Collections.ObjectModel;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class DeformedShapeSymmetrySetViewModel : ViewModelBase
    {
        public ObservableCollection<DeformedShapeSymmetryViewModel> SymmetrySets { get; } = [];
    }
}
