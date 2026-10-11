using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ColorMapViewModel : ViewModelBase
    {
        private IColorMap selectedColorMap;

        public event EventHandler? SelectedColorMapChanged;

        public List<IColorMap> ColorMaps { get; set; }

        public IColorMap SelectedColorMap
        {
            get => selectedColorMap;
            set
            {
                if (EqualityComparer<IColorMap>.Default.Equals(
                    selectedColorMap, value))
                    return;

                selectedColorMap = value;

                OnPropertyChanged(nameof(SelectedColorMap));
                OnSelectedColorMapChanged();
            }
        }

        public ColorMapViewModel()
        {
            ColorMaps =
            [
                ColorMapFactory.GetColorMap(ColorMapsTypes.LiraSpectrum),
                ColorMapFactory.GetColorMap(ColorMapsTypes.FullSpectrum),
                ColorMapFactory.GetColorMap(ColorMapsTypes.RedToWhite),
                ColorMapFactory.GetColorMap(ColorMapsTypes.RedToBlue),
                ColorMapFactory.GetColorMap(ColorMapsTypes.BlueToWhite),
                ColorMapFactory.GetColorMap(ColorMapsTypes.BlackToWhite)
            ];

            SelectedColorMap = ColorMaps[0];
        }

        protected virtual void OnSelectedColorMapChanged()
        {
            SelectedColorMapChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}