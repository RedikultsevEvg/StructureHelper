using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IVisualProperty
    {
        /// <summary>
        /// Flag of visibility
        /// </summary>
        bool IsVisible { get; set; }
        Color Color { get; set; }
        /// <summary>
        /// Flag of assigning of color from material or from primitive's settings
        /// </summary>
        bool SetMaterialColor { get; set; }
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
