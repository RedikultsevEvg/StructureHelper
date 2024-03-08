using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    /// <inheritdoc/>
    public class ValueColorRange : IValueColorRange
    {
        /// <inheritdoc/>
        public bool IsActive { get; set; }
        /// <inheritdoc/>
        public double BottomValue { get; set; }
        /// <inheritdoc/>
        public double AverageValue { get; set; }
        /// <inheritdoc/>
        public double TopValue { get; set; }
        /// <inheritdoc/>
        public Color BottomColor { get; set; }
        /// <inheritdoc/>
        public Color TopColor { get; set; }
    }
}
