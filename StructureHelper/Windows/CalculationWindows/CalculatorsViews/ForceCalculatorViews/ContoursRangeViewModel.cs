using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.ValueRanges;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ContoursRangeViewModel : ViewModelBase
    {
        const int RangeNumber = 16;
        private double userMinValue;
        private double userMaxValue;

        public IColorMap? ColorMap { get; set; }
        public IValueRange? ValueRange { get; set; } = new ValueRange();
        public bool SetMinValue { get; set; } = false;
        public double UserMinValue
        {
            get => userMinValue;
            set
            {
                userMinValue = value;
                Refresh();
            }
        }
        public bool SetMaxValue { get; set; } = false;
        public double UserMaxValue
        {
            get => userMaxValue;
            set
            {
                userMaxValue = value;
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
        }
    }
}
