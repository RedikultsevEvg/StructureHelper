using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    /// <summary>
    /// Colored range for building color legend
    /// </summary>
    public interface IValueColorRange
    {
        /// <summary>
        /// Flag of activity
        /// </summary>
        bool IsActive { get; set; }
        IValueColorArray ExactValues { get; }
        IValueColorArray RoundedValues { get; }

    }
}
