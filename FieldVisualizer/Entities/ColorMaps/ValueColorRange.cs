using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    /// <inheritdoc/>
    public class ValueColorRange : IValueColorRange
    {
        /// <inheritdoc/>
        public bool IsActive { get; set; }

        public IValueColorArray ExactValues { get; private set; } = new ValueColorArray();

        public IValueColorArray RoundedValues { get; private set; } = new ValueColorArray();

    }
}
