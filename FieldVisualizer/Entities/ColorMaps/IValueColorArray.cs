using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    public interface IValueColorArray
    {
        double AverageValue { get; set; }
        Color BottomColor { get; set; }
        double BottomValue { get; set; }
        Color TopColor { get; set; }
        double TopValue { get; set; }
    }
}