using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoFieldTitleViewModel : ViewModelBase
    {
        private IPrimitiveSet primitiveSet;

        public string Title { get; set; }
        public string SubTitle { get; set; }
        

        public IsoFieldTitleViewModel(IPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
            Title = primitiveSet.Name;
            SubTitle = primitiveSet.SubTitle;
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(SubTitle));
        }
    }
}
