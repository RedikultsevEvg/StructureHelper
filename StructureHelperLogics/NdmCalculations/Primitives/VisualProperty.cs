using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class VisualProperty : IVisualProperty
    {
        
        public bool IsVisible { get; set; }
        public Color Color { get; set; }
        public bool SetMaterialColor { get; set; }
        public int ZIndex { get; set; }
        private double opacity;

        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (value < 0d || value > 1d) { throw new StructureHelperException(ErrorStrings.VisualPropertyIsNotRight + nameof(Opacity) + value); } 
                opacity = value;
            }
        }


        public VisualProperty()
        {
            IsVisible = true;
            Color = ColorProcessor.GetRandomColor();
            SetMaterialColor = true;
            ZIndex = 0;
            Opacity = 1;
        }
    }
}
