using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Models.VisualProperties
{
    /// <summary>
    /// Implements visual settings for graphical primetives
    /// </summary>
    public interface IPrimitiveVisualProperty : ISaveable, ICloneable
    {
        /// <summary>
        /// Flag of visibility
        /// </summary>
        bool IsVisible { get; set; }
        /// <summary>
        /// Color of primitive
        /// </summary>
        Color Color { get; set; }
        /// <summary>
        /// Index by z-coordinate
        /// </summary>
        int ZIndex { get; set; }
        /// <summary>
        /// Opacity of filling
        /// </summary>
        double Opacity { get; set; }
    }
}
