using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps.Factories
{
    public enum ColorMapsTypes
    {
        LiraSpectrum = 0, //Lira
        FullSpectrum = 1, //StaDiCon
        RedToWhite = 2,
        RedToBlue = 3,
        BlueToWhite = 4,
        BlackToWhite = 5,
    }
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
            if (mapsTypes == ColorMapsTypes.BlackToWhite) { return GetBlackToWhite(); }
            if (mapsTypes == ColorMapsTypes.LiraSpectrum) { return GetLiraSpectrum(); }
            else { throw new FieldVisulizerException(ErrorStrings.ColorMapTypeIsUnknown); }
        }

        private static IColorMap GetLiraSpectrum()
        {
            ColorMap colorMap = new()
            {
                Name = "Lira Style Spectrum"
            };
            List<Color> colors = new();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0, 0, 128) ,//Dark Blue
                Color.FromArgb(Alpha, 0, 0, 255) ,//Blue
                Color.FromArgb(Alpha, 0, 128, 255) ,//Blue
                Color.FromArgb(Alpha, 0, 200, 255) ,//Blue
                Color.FromArgb(Alpha, 60, 255, 255) ,//Light Blue
                Color.FromArgb(Alpha, 255, 255, 128) ,//Light Yellow
                Color.FromArgb(Alpha, 255, 255, 0) ,//Yellow
                Color.FromArgb(Alpha, 255, 215, 0) ,//Gold
                Color.FromArgb(Alpha, 255, 128, 0) ,//Orange Red
                Color.FromArgb(Alpha, 255, 0, 0) ,//Red
            });
            colorMap.Colors = colors;
            return colorMap;
        }
        private static IColorMap GetFullSpectrum()
        {
            const int colorCount = 11;
            const byte alpha = 0xFF;

            ColorMap colorMap = new()
            {
                Name = "Full Spectrum"
            };

            List<Color> colors = new();

            double startHue = 270;     // Blue (avoid wrapping back to red)
            double endHue = 0;     // Red

            for (int i = 0; i < colorCount; i++)
            {
                double t = (double)i / (colorCount - 1);
                double hue = startHue + t * (endHue - startHue);

                colors.Add(ColorFromHSV(
                    hue,
                    saturation: 1.0,
                    value: 1.0,
                    alpha: alpha));
            }

            colorMap.Colors = colors;
            return colorMap;
        }

        private static IColorMap GetRedToWhite()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "Red To White Spectrum";
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
            colorMap.Name = "Red To Blue Spectrum";
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
            colorMap.Name = "Blue To White Spectrum";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0, 0, 0xFF) ,//Blue
                Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) ,//White
            });
            colorMap.Colors = colors;
            return colorMap;
        }

        private static IColorMap GetBlackToWhite()
        {
            ColorMap colorMap = new ColorMap();
            colorMap.Name = "Black To White Spectrum";
            List<Color> colors = new List<Color>();
            byte Alpha = 0xff;
            colors.AddRange(new Color[]{
                Color.FromArgb(Alpha, 0, 0, 0) ,//Black
                Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) ,//White
            });
            colorMap.Colors = colors;
            return colorMap;
        }

        private static Color ColorFromHSV(double hue, double saturation, double value, byte alpha = 0xFF)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            byte v = (byte)value;
            byte p = (byte)(value * (1 - saturation));
            byte q = (byte)(value * (1 - f * saturation));
            byte t = (byte)(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromArgb(alpha, v, t, p),
                1 => Color.FromArgb(alpha, q, v, p),
                2 => Color.FromArgb(alpha, p, v, t),
                3 => Color.FromArgb(alpha, p, q, v),
                4 => Color.FromArgb(alpha, t, p, v),
                _ => Color.FromArgb(alpha, v, p, q),
            };
        }

    }
}
