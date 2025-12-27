using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoField2DViewerViewModel : ViewModelBase
    {
        private SelectedPrimitiveSet ndmPrimitiveSet;

        public IsoField2DViewerView Window { get; internal set; }
        public IsoFieldTitle Title { get; set; }
        public IsoFieldSummary Summary { get; }
        public List<IPrimitiveSet> PrimitiveSets { get; set; }
        public IPrimitiveSet SelectedPrimitiveSet { get; set; }
        public ColorMapViewModel ColorMapViewModel { get; set; }
        public ContoursRangeViewModel ContourRange { get; set; } = new();

        public IsoField2DViewerViewModel(SelectedPrimitiveSet ndmPrimitiveSet)
        {
            this.ndmPrimitiveSet = ndmPrimitiveSet;
            SetValues(ndmPrimitiveSet);
            ColorMapViewModel = new ColorMapViewModel(this);
            SelectedPrimitiveSet = PrimitiveSets[0];
            Refresh();
        }

        private void SetValues(SelectedPrimitiveSet ndmPrimitiveSet)
        {
            PrimitiveSets = ShowIsoFieldResult.GetPrimitiveSets(ndmPrimitiveSet.StrainMatrix, ndmPrimitiveSet.Ndms, ForceResultFuncFactory.GetResultFuncs());
        }

        public void Refresh()
        {
            if (SelectedPrimitiveSet is null) { return; }
            Title = new(SelectedPrimitiveSet);
            Title.Refresh();
            OnPropertyChanged(nameof(Title));
            if (ColorMapViewModel is null) { return; }
            ContourRange.ColorMap = ColorMapViewModel.SelectedColorMap;
            double minValue = SelectedPrimitiveSet.ValuePrimitives.Min(x => x.Value);
            ContourRange.ValueRange.BottomValue = ContourRange.UserMinValue = minValue;
            double maxValue = SelectedPrimitiveSet.ValuePrimitives.Max(x => x.Value);
            ContourRange.ValueRange.TopValue = ContourRange.UserMinValue = maxValue;
            ContourRange.Refresh();
            OnPropertyChanged(nameof(ContourRange));
        }
    }
}
