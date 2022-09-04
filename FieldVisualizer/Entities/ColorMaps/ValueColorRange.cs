using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    public class ValueColorRange : IValueColorRange
    {
        public bool IsActive { get; set; }
        public double BottomValue { get; set; }
        public double AverageValue { get; set; }
        public double TopValue { get; set; }
        public Color BottomColor { get; set; }
        public Color TopColor { get; set; }
    }
}
