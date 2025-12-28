using FieldVisualizer.Entities.ColorMaps;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Converters;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ContourLegendViewModel : ViewModelBase
    {
        public ObservableCollection<IValueColorRange>? ValueColorRanges { get; set; } = new();
        public double MaxValue => ValueColorRanges[^1].RoundedValues.TopValue;
        public double MinValue => ValueColorRanges[0].RoundedValues.BottomValue;

        public void Refresh()
        {
            OnPropertyChanged(nameof(MaxValue));
            OnPropertyChanged(nameof(MinValue));
        }
    }
}
