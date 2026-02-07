using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.ValueRanges;
using StructureHelper.Infrastructure;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ContoursRangeViewModel : ViewModelBase
    {
        public List<int> RangeNumbers => [4, 6, 8, 12, 16];
        public int RangeNumber { get; set; } = 8;
        private double userMinValue;
        private double userMaxValue;

        public IColorMap? ColorMap { get; set; }
        public IValueRange? ValueRange { get; set; } = new ValueRange();
        public double UserMinValue
        {
            get => userMinValue;
            set
            {
                userMinValue = value;
                ValueRange.BottomValue = userMinValue;
                Refresh();
            }
        }
        public double UserMaxValue
        {
            get => userMaxValue;
            set
            {
                userMaxValue = value;
                ValueRange.TopValue = userMaxValue;
                Refresh();
            }
        }

        public ContourLegendViewModel ContourLegend { get; set; } = new();
        public ContoursRangeViewModel()
        {
            //Refresh();
        }

        public void Refresh()
        {
            var valueRanges = ValueRangeOperations.DivideValueRange(ValueRange, RangeNumber);
            var valueColorRanges = ColorOperations.GetValueColorRanges(ValueRange, valueRanges, ColorMap);
            ContourLegend.ValueColorRanges.Clear();
            foreach (var valueRange in valueColorRanges)
            {
                ContourLegend.ValueColorRanges.Add(valueRange);
            }
            OnPropertyChanged(nameof(UserMinValue));
            OnPropertyChanged(nameof(UserMaxValue));
            ContourLegend.Refresh();
        }
    }
}
