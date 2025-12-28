using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class ColorMapViewModel : ViewModelBase
    {
        private IsoField2DViewerViewModel isoField2DViewerViewModel;
        private IColorMap selectedColorMap;

        public List<IColorMap> ColorMaps { get; set; }
        public IColorMap SelectedColorMap
        {
            get => selectedColorMap;
            set
            {
                selectedColorMap = value;
                Refresh();
            }
        }
        public ColorMapViewModel(IsoField2DViewerViewModel isoField2DViewerViewModel)
        {
            this.isoField2DViewerViewModel = isoField2DViewerViewModel;
            ColorMaps = [];
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.LiraSpectrum));
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.FullSpectrum));
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.RedToWhite));
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.RedToBlue));
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.BlueToWhite));
            ColorMaps.Add(ColorMapFactory.GetColorMap(ColorMapsTypes.BlackToWhite));
            SelectedColorMap = ColorMaps[0];
        }
        private void Refresh()
        {
            isoField2DViewerViewModel.Refresh();
        }

    }
}
