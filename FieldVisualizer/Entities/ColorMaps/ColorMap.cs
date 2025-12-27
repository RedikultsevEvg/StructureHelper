using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using System.Windows;

namespace FieldVisualizer.Entities.ColorMaps
{
    public class ColorMap : IColorMap
    {
        private LinearGradientBrush _gradientBrush;

        public string Name { get; set; }
        public List<Color> Colors { get; set; }

        public LinearGradientBrush GradientBrush
        {
            get
            {
                if (_gradientBrush == null)
                {
                    _gradientBrush = CreateGradientBrush();
                    _gradientBrush.Freeze(); // very important for performance
                }

                return _gradientBrush;
            }
        }

        protected virtual LinearGradientBrush CreateGradientBrush()
        {
            var brush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0) // horizontal
            };

            int count = Colors.Count;
            for (int i = 0; i < count; i++)
            {
                brush.GradientStops.Add(
                    new GradientStop(
                        Colors[i],
                        (double)i / (count - 1)));
            }

            return brush;
        }
    }
}
