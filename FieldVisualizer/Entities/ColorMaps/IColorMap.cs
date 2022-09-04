using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace FieldVisualizer.Entities.ColorMaps
{
    public interface IColorMap
    {
        string Name { get;}
        List<Color> Colors { get; }
    }
}
