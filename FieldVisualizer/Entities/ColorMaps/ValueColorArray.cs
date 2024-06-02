using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FieldVisualizer.Entities.ColorMaps
{
    public class ValueColorArray : IValueColorArray
    {
        /// <summary>
        /// Minimum value of range
        /// </summary>
        public double BottomValue { get; set; }
        /// <summary>
        /// Average value of range
        /// </summary>
        public double AverageValue { get; set; }
        /// <summary>
        /// Maximum value of range
        /// </summary>
        public double TopValue { get; set; }
        /// <summary>
        /// Color correspondent to minimum value
        /// </summary>
        public Color BottomColor { get; set; }
        /// <summary>
        /// Color correspondent to maximum value
        /// </summary>
        public Color TopColor { get; set; }
    }
}
