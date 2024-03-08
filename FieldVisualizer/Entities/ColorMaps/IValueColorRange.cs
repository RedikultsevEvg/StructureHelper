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
        /// <summary>
        /// Minimum value of range
        /// </summary>
        double BottomValue { get; set; }
        /// <summary>
        /// Average value of range
        /// </summary>
        double AverageValue { get; set; }
        /// <summary>
        /// Maximum value of range
        /// </summary>
        double TopValue {get;set;}
        /// <summary>
        /// Color correspondent to minimum value
        /// </summary>
        Color BottomColor { get; set; }
        /// <summary>
        /// Color correspondent to maximum value
        /// </summary>
        Color TopColor { get; set; }
    }
}
