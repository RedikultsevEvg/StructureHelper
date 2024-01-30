using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps.Factories
{
    /// <summary>
    /// Factory for creating of different color maps
    /// </summary>
    public static class ColorMapFactory
    {
        public static IColorMap GetColorMap(ColorMapsTypes mapsTypes)
        {
            if (mapsTypes == ColorMapsTypes.FullSpectrum) { return GetFullSpectrum(); }
            if (mapsTypes == ColorMapsTypes.RedToWhite) { return GetRedToWhite(); }
            if (mapsTypes == ColorMapsTypes.RedToBlue) { return GetRedToBlue(); }
            if (mapsTypes == ColorMapsTypes.BlueToWhite) { return GetBlueToWhite(); }
            else { throw new FieldVisulizerException(ErrorStrings.ColorMapTypeIsUnknown); }
        }

        private static IColorMap GetFullSpectrum()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "FullSpectrumColorMap";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0xFF, 0x80, 0x80) ,//
                Color.FromArgb(Alpha, 0xFF, 0, 0x80) ,//
                Color.FromArgb(Alpha, 0xFF, 0, 0) ,//Red
                Color.FromArgb(Alpha, 0xFF, 0x45, 0) ,//Orange Red
                Color.FromArgb(Alpha, 0xFF, 0xD7, 0) ,//Gold
                Color.FromArgb(Alpha, 0xFF, 0xFF, 0) ,//Yellow
                Color.FromArgb(Alpha, 0x9A, 0xCD, 0x32) ,//Yellow Green
                Color.FromArgb(Alpha, 0, 0x80, 0) ,//Green
                Color.FromArgb(Alpha, 0, 0x64, 0) ,//Dark Green
                Color.FromArgb(Alpha, 0x2F, 0x4F, 0x4F) ,//Dark Slate Gray
                Color.FromArgb(Alpha, 0, 0, 0xFF) ,//Blue
            });
            colorMap.Colors = colors;
            return colorMap;
        }

        private static IColorMap GetRedToWhite()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "FullSpectrumColorMap";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0xFF, 0, 0) ,//Red
                Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) ,//White
            });
            colorMap.Colors = colors;
            return colorMap;
        }

        private static IColorMap GetRedToBlue()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "FullSpectrumColorMap";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0xFF, 0, 0) ,//Red
                Color.FromArgb(Alpha, 0, 0, 0xFF) ,//Blue
            });
            colorMap.Colors = colors;
            return colorMap;
        }

        private static IColorMap GetBlueToWhite()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "FullSpectrumColorMap";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0, 0, 0xFF) ,//Blue
                Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) ,//White
            });
            colorMap.Colors = colors;
            return colorMap;
        }
    }
}
