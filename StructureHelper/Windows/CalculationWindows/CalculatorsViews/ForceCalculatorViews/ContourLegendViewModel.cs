using FieldVisualizer.Entities.ColorMaps;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ContourLegendViewModel : ViewModelBase
    {
        public ObservableCollection<IValueColorRange>? ValueColorRanges { get; set; } = new();
    }
}
