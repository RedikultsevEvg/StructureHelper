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
        bool IsVisible { get; set; }
        Color Color { get; set; }
        bool SetMaterialColor { get; set; }
        int ZIndex { get; set; }
        double Opacity { get; set; }
    }
}
