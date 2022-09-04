using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    public interface IValueColorRange
    {
        bool IsActive { get; set; }
        double BottomValue { get; set; }
        double AverageValue { get; set; }
        double TopValue {get;set;}
        Color BottomColor { get; set; }
        Color TopColor { get; set; }
    }
}
