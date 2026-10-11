using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ContourColorMap : ViewModelBase
    {
        private double minValue;
        private double maxValue;

        public double MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                Refresh();
                OnPropertyChanged(nameof(MinValue));
            }
        }
        public double MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                Refresh();
                OnPropertyChanged(nameof(MaxValue));
            }
        }
        public ColorMapViewModel ColorMap { get; }
        public ContoursRangeViewModel ContourRange { get; } = new();

        public event EventHandler? Changed;

        public ContourColorMap()
        {
            ColorMap = new ColorMapViewModel();
            ColorMap.SelectedColorMapChanged += OnSelectedColorMapChanged;
            Refresh();
        }

        public void Refresh()
        {
            ContourRange.ColorMap = ColorMap.SelectedColorMap;
            ContourRange.UserMinValue = minValue;
            ContourRange.UserMaxValue = maxValue;
            ContourRange.Refresh();
        }

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void OnSelectedColorMapChanged(object? sender, EventArgs e)
        {
            Refresh();
            OnChanged();
        }
    }
}
