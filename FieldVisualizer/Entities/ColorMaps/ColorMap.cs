using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace FieldVisualizer.Entities.ColorMaps
{
    public class ColorMap : IColorMap
    {
        public string Name { get; set; }
        public List<Color> Colors { get; set; }
    }
}
